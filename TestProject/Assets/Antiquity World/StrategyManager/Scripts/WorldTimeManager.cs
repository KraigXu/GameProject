using System;
using System.Collections.Generic;
using Chronos;
using UnityEngine;

namespace GameSystem
{


    public class WorldTimerNode
    {
        public int Id;
        public bool IsActive;

        public bool IsStart;
        public DateTime StartTime;
        public Action StartCallback;

        public DateTime EndTime;
        public Action EndCallback;

        public WorldTimerNode(int id, DateTime startTime, Action startCallback, DateTime endTime, Action endcallback)
        {
            this.Id = id;
            this.StartTime = startTime;
            this.StartCallback = startCallback;
            this.EndTime = endTime;
            this.EndCallback = endcallback;
            this.IsActive = true;
        }

        public void Tick(DateTime time)
        {
            if (time >= StartTime && IsStart == false)
            {
                StartCallback.Invoke();
                IsStart = true;
            }

            if (time >= EndTime)
            {
                EndCallback.Invoke();
                IsActive = false;
                WorldTimeManager.Instance.RemoveTimer(Id);
            }
        }
    }

    public enum TimeSatus
    {
        Play,
        Stop,
    }


    /// <summary>
    /// 时间管理
    /// </summary>
    public class WorldTimeManager : MonoBehaviour
    {
        private static WorldTimeManager _instance;

        public static WorldTimeManager Instance
        {
            get { return _instance; }
        }

        public bool IsInit = false;
        public bool IsPlay = false;
        public bool IsStart = false;

        public DateTime CurTime = Convert.ToDateTime("1000-01-01 00:00:00");
        public int Year = 1000;
        public int Month = 1;
        public int Day = 1;
        public int Shichen = 1;
        public int Hour = 0;

        public int Jijie;

        public byte TimeScalar = 1;           //时间的放大比 如果是0 则是暂停
        public byte ScheduleCell = 5;         //时间节点的大小
        public float Schedule = 0;             //一个时间节点的进度
        public byte ScheduleMonth = 0;
        public byte ScheduleDay = 0;
        public byte ScheduleYear = 0;

        public GlobalClock StrategyClock;
        public List<WorldTimerNode> WorldTimerNodes = new List<WorldTimerNode>();

        private List<int> _removalPending = new List<int>();
        private int _idCounter;

        public int AddTime = 0;
        public float delay = 0;


        private Dictionary<int, string> TimeJijie = new Dictionary<int, string>();

        private Dictionary<int, string> TimeShichen = new Dictionary<int, string>();


        //        子时 丑时  寅时 卯时  辰时 巳时
        //        23:00 - 00:59 01:00 - 02:59 03:00 - 04:59 05:00 - 06:59 07:00 - 08:59 09:00 - 10:59
        //        午时 未时  申时 酉时  戊时 亥时
        //        11:00 - 12:59 13:00 - 14:59 15:00 - 16:59 17:00 - 18:59 19:00 - 20:59 21:00 - 22:59

        void Awake()
        {
            _instance = this;

            TimeJijie.Add(1, "春");
            TimeJijie.Add(2, "夏");
            TimeJijie.Add(3, "秋");
            TimeJijie.Add(4, "冬");

            TimeShichen.Add(1, "子时");
            TimeShichen.Add(2, "丑时");
            TimeShichen.Add(3, "寅时");
            TimeShichen.Add(4, "卯时");
            TimeShichen.Add(5, "辰时");
            TimeShichen.Add(6, "巳时");
            TimeShichen.Add(7, "午时");
            TimeShichen.Add(8, "未时");
            TimeShichen.Add(9, "申时");
            TimeShichen.Add(10, "酉时");
            TimeShichen.Add(11, "戊时");
            TimeShichen.Add(12, "亥时");

            DateTime time = Convert.ToDateTime("1000-01-01 00:00:00");
            Init(time);
        }

        public void Init(DateTime dataTime)
        {
            if (IsInit == false)
            {
                CurTime = dataTime;
                Year = CurTime.Year;
                Month = CurTime.Month;
                Day = CurTime.Day;
                Hour = CurTime.Hour;
                Shichen = UpdateGuDaiTime(Hour);
                Jijie = UpdateTimeJijie(Month);

                IsInit = true;
                IsPlay = true;
            }
            else
            {
            }
        }

        enum TimePlay
        {
            Play, Pause,
        }


        TimeSatus CurTimeSatus;
        public DateTime Times;

        public string YearS
        {
            get { return CurTime.Year.ToString(); }
        }


        public string DayS
        {
            get { return CurTime.Day.ToString(); }
        }

        public string MonthS
        {
            get { return CurTime.Month.ToString(); }
        }


        void Update()
        {

            if (AddTime > 0)
            {
                delay += Time.deltaTime;

                if (delay > 1)
                {
                    AddTime--;
                    CurTime = CurTime.AddDays(1);
                    delay = 0;
                }

            }


            //if (StrategyClock.paused == false || IsStart == false || IsInit == false)
            //{
            //    return;
            //}

            //float dt = Time.deltaTime;
            //Schedule += dt * TimeScalar;
            //if (Schedule >= ScheduleCell)   //表示一个时间节点完成
            //{
            //    Schedule = 0;
            //    CurTime = CurTime.AddHours(2);
            //    Year = CurTime.Year;
            //    Month = CurTime.Month;
            //    Day = CurTime.Day;
            //    Shichen = UpdateGuDaiTime(CurTime.Hour);
            //    Jijie = UpdateTimeJijie(CurTime.Month);
            //    ScheduleDay++;

            //    //AddPeriodValue(PeriodType.Shichen);

            //    if (ScheduleDay > 12)
            //    {
            //        ScheduleDay = 0;
            //        ScheduleMonth++;
            //        //  AddPeriodValue(PeriodType.Day);
            //        if (ScheduleMonth > 31)
            //        {
            //            ScheduleMonth = 0;
            //            // AddPeriodValue(PeriodType.Month);
            //            ScheduleYear++;
            //            if (ScheduleYear > 12)
            //            {
            //                // AddPeriodValue(PeriodType.Year);
            //                ScheduleYear = 0;
            //            }
            //        }
            //    }
            //}
        }

        private int UpdateGuDaiTime(int curHour)
        {
            if (curHour == 23 || curHour == 0)
                return 1;
            else if (curHour == 1 || curHour == 2)
                return 2;
            else if (curHour == 3 || curHour == 4)
                return 3;
            else if (curHour == 5 || curHour == 6)
                return 4;
            else if (curHour == 7 || curHour == 8)
                return 5;
            else if (curHour == 9 || curHour == 10)
                return 6;
            else if (curHour == 11 || curHour == 12)
                return 7;
            else if (curHour == 13 || curHour == 14)
                return 8;
            else if (curHour == 15 || curHour == 16)
                return 9;
            else if (curHour == 17 || curHour == 18)
                return 10;
            else if (curHour == 19 || curHour == 20)
                return 11;
            else if (curHour == 21 || curHour == 22)
                return 12;
            else
                return 1;
        }
        private int UpdateTimeJijie(int curMonth)
        {
            if (curMonth == 2 || curMonth == 3 || curMonth == 4)
            {
                return 1;
            }
            else if (curMonth == 5 || curMonth == 6 || curMonth == 7)
            {
                return 2;
            }
            else if (curMonth == 8 || curMonth == 9 || curMonth == 10)
            {
                return 3;
            }
            else if (curMonth == 11 || curMonth == 12 || curMonth == 1)
            {
                return 4;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 创建新的时间序列
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="startAction"></param>
        /// <param name="endTime"></param>
        /// <param name="endAction"></param>
        /// <returns></returns>
        public int AddTimerNode(DateTime startTime, Action startAction, DateTime endTime, Action endAction)
        {
            WorldTimerNode node = new WorldTimerNode(++_idCounter, startTime, startAction, endTime, endAction);
            WorldTimerNodes.Add(node);
            return node.Id;
        }


        /// <summary>
        /// 删除时间序列
        /// </summary>
        /// <param name="timerId">时间ID</param>
        public void RemoveTimer(int timerId)
        {
            _removalPending.Add(timerId);
        }

        /// <summary>
        /// 定时器删除队列处理程序
        /// </summary>
        private void Remove()
        {
            if (_removalPending.Count > 0)
            {
                foreach (int id in _removalPending)
                {
                    for (int i = 0; i < WorldTimerNodes.Count; i++)
                    {
                        if (WorldTimerNodes[i].Id == id)
                        {
                            WorldTimerNodes.RemoveAt(i);
                            break;
                        }
                    }
                }
                _removalPending.Clear();
            }
        }


        public void Play()
        {
            IsPlay = true;
        }

        public void Pause()
        {
            IsPlay = false;
        }


        public void InitTimeData(DateTime time)
        {
            CurTime = time;
            Year = CurTime.Year;
            Month = CurTime.Month;
            Day = CurTime.Day;
            Shichen = UpdateGuDaiTime(CurTime.Hour);
            Jijie = UpdateTimeJijie(CurTime.Month);

            IsInit = true;
        }

        public string ShiChen
        {
            get { return TimeShichen[Shichen]; }
        }

        public string Season
        {
            get { return TimeJijie[Jijie]; }
        }

        public static void AddDay(int number)
        {
            Instance.AddTime += number;
        }
    }
}
