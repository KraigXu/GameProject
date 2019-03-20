using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class WorldTimeSystem : ComponentSystem
    {
        struct PeriodTimeGroup
        {
            public readonly int Length;
            public ComponentDataArray<PeriodTime> PeriodTime;
            
        }

        [Inject]
        private PeriodTimeGroup _period;

        public static DateTime CurTime;
        public static int Year = 1000;
        public static int Month = 1;
        public static int Day = 1;
        public static int Shichen = 1;

        public static int Jijie;

        public static byte TimeScalar = 1;           //时间的放大比 如果是0 则是暂停
        public static byte ScheduleCell = 5;         //时间节点的大小
        public static float Schedule = 0;             //一个时间节点的进度
        public static byte ScheduleMonth = 0;
        public static byte ScheduleDay = 0;
        public static byte ScheduleYear = 0;

        public static bool IsInit = false;
        public static bool IsStart=true;

        public static void InitTimeData(DateTime time)
        {
            CurTime = time;
            Year = CurTime.Year;
            Month = CurTime.Month;
            Day = CurTime.Day;
            Shichen = UpdateGuDaiTime(CurTime.Hour);
            Jijie = UpdateTimeJijie(CurTime.Month);

            IsInit = true;
        }


        protected override void OnUpdate()
        {
            if (IsStart == false ||IsInit==false)
            {
                return;
            }

            float dt = Time.deltaTime;
            Schedule += dt * TimeScalar;
            if (Schedule >= ScheduleCell)   //表示一个时间节点完成
            {
                Schedule = 0;
                CurTime = CurTime.AddHours(2);
                Year = CurTime.Year;
                Month = CurTime.Month;
                Day = CurTime.Day;
                Shichen = UpdateGuDaiTime(CurTime.Hour);
                Jijie = UpdateTimeJijie(CurTime.Month);
                ScheduleDay++;
                AddPeriodValue(PeriodType.Shichen);

                if (ScheduleDay > 12)
                {
                    ScheduleDay = 0;
                    ScheduleMonth++;
                    AddPeriodValue(PeriodType.Day);
                    if (ScheduleMonth > 31)
                    {
                        ScheduleMonth = 0;
                        AddPeriodValue(PeriodType.Month);
                        ScheduleYear++;
                        if (ScheduleYear > 12)
                        {
                            AddPeriodValue(PeriodType.Year);
                            ScheduleYear = 0;
                        }
                    }
                }
            }
        }

        public void AddPeriodValue(PeriodType type)
        {
            for (int i = 0; i < _period.Length; i++)
            {
                var period = _period.PeriodTime[i];
                if (period.Type == type)
                {
                    period.Value++;
                    _period.PeriodTime[i] = period;
                }
            }
        }

        private static int UpdateGuDaiTime(int curHour)
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

        private static int UpdateTimeJijie(int curMonth)
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

    }


}
