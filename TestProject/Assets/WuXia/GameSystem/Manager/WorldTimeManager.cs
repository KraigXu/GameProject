using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GameSystem
{
    public enum TimeSatus
    {
        Play,
        Stop,
    }

    public class WorldTimerNode
    {
        public int Id;
        public bool IsActive;

        public DateTime StartTime;
        public Action StartCallback;
        public bool IsStart=false;

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
            if (time>=StartTime &&IsStart==false)
            {
                StartCallback.Invoke();
                IsStart = true;
            }

            if (time>=EndTime)
            {
                EndCallback.Invoke();
                IsActive = false;
                WorldTimeManager.Instance.RemoveTimer(Id);
            }
        }
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
        public DateTime CurTime;
        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Shichen;
        public int Jijie;

        public byte TimeScalar=1;           //时间的放大比 如果是0 则是暂停
        public float Schedule=0;             //一个时间节点的进度
        public byte ScheduleCell=5;         //时间节点的大小


        public List<WorldTimerNode>  WorldTimerNodes=new List<WorldTimerNode>() ;

        private List<int> _removalPending=new List<int>();
        private int _idCounter;


        void Awake()
        {
            _instance = this;
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

        void Update()
        {
            if (IsInit == false)
            {
                return;
            }

            if (IsPlay == false)
            {
                return;
            }

            float dt = Time.deltaTime;

            Schedule += dt * TimeScalar;

            if (Schedule >= ScheduleCell)
            {
                Schedule = 0;
                CurTime = CurTime.AddHours(1);
                Year = CurTime.Year;
                Month = CurTime.Month;
                Day = CurTime.Day;
                Hour = CurTime.Hour;
                Shichen = UpdateGuDaiTime(Hour);
                Jijie = UpdateTimeJijie(Month);
            }

            Remove();
            for (int i = 0; i < WorldTimerNodes.Count; i++)
            {
                WorldTimerNodes[i].Tick(CurTime);
            }
            
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
            WorldTimerNode node=new WorldTimerNode(++_idCounter,startTime,startAction,endTime,endAction);
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

    }
}
