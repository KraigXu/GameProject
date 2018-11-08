using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace WX
{

    public enum TimeSpeed
    {
        Normal = 1,  //正常
        Slow = 2,   //慢速
        Extreme = 3  //快速
    }

    public enum TimeSatus
    {
        Play,
        Stop,
    }
    /// <summary>
    /// 时间管理  ，包含事件，
    /// </summary>
    public class WorldTimeSystem : ComponentSystem
    {


        public DateTime CurTime;

        public int curYera = 194;
        public int curMonth = 1;
        public int curDay = 1;
        public int curHour = 1;
        public string curGd = "";
        public string curSeason = "";
        public string curJieQi = "";
        public TimeSatus curStatus;
        public TimeSpeed curSpeed;

        public float TimeScalar = 5;         //时间标量，以秒为单位
        public float curSchedule = 0;        //当前进度，当进度>=时间标量时则计算为一小时  ，时间速率分别为  {正常=时间标量*1,  慢速=时间标量*2,快速=时间标量*0.5}
        public float curSpeedValue = 1;

        public bool IsResetTime = true;

        //        子时 丑时  寅时 卯时  辰时 巳时
        //        23:00 - 00:59 01:00 - 02:59 03:00 - 04:59 05:00 - 06:59 07:00 - 08:59 09:00 - 10:59
        //        午时 未时  申时 酉时  戊时 亥时
        //        11:00 - 12:59 13:00 - 14:59 15:00 - 16:59 17:00 - 18:59 19:00 - 20:59 21:00 - 22:59
        public const string Zishi = "子时";
        public const string Choshi = "丑时";
        public const string Yinshi = "寅时";
        public const string Moshi = "卯时";
        public const string Chenshi = "辰时";
        public const string Sishi = "巳时";
        public const string Wushi = "午时";
        public const string Weishi = "未时";
        public const string Shenshi = "申时";
        public const string Youshi = "酉时";
        public const string Shushi = "戊时";
        public const string Haishi = "亥时";

        public static DateTime CurWorldTime;

        struct TimeData
        {
            public WorldTimeUi WorldTimeUI;
        }


        //public ov

        public static void SetupComponentData(EntityManager entityManager)
        {


            //if (IsResetTime)  //重置为当前系统时间 转化为古代时间
            //{
            //    CurTime = DateTime.Now;
            //    UpdateGuDaiTime();
            //}
            //else            //取得剧本时间
            //{
            //    CurTime = new DateTime(1994, 1, 3, 12, 0, 0);
            //    UpdateGuDaiTime();
            //}
        }

        struct TimeUi
        {
            public WorldTimeUi time;
        }

        protected override void OnUpdate()
        {
            CurTime = StrategySceneInit.Settings.curTime;
            if (curStatus == TimeSatus.Play)
            {
                curSchedule += (Time.deltaTime * curSpeedValue);
                if (curSchedule >= TimeScalar)
                {
                    curSchedule = 0;

                    CurTime = CurTime.AddHours(1);

                    curYera = CurTime.Year;
                    curMonth = CurTime.Month;
                    curDay = CurTime.Day;
                    curHour = CurTime.Hour;
                    curGd = UpdateGuDaiTime();
                    curJieQi = "";
                    if (curMonth == 2 || curMonth == 3 || curMonth == 4)
                    {
                        curSeason = "春";
                    }
                    else if (curMonth == 5 || curMonth == 6 || curMonth == 7)
                    {
                        curSeason = "夏";
                    }
                    else if (curMonth == 8 || curMonth == 9 || curMonth == 10)
                    {
                        curSeason = "秋";
                    }
                    else if (curMonth == 11 || curMonth == 12 || curMonth == 1)
                    {
                        curSeason = "冬";
                    }

                }
            }
            else if (curStatus == TimeSatus.Stop)
            {
            }

            foreach (var v in GetEntities<TimeUi>())
            {
                v.time.Year.text = curYera.ToString();
                v.time.Month.text = curMonth.ToString();
                v.time.Day.text = curDay.ToString();
                v.time.ShiChen.text = curGd.ToString();
               // v.time.JieQi.text = "";
                v.time.Season.text = curSeason;
            }
            StrategySceneInit.Settings.curTime = CurTime;
        }

        private string UpdateGuDaiTime()
        {
            if (curHour == 23 || curHour == 0)
                return Zishi;
            else if (curHour == 1 || curHour == 2)
                return Choshi;
            else if (curHour == 3 || curHour == 4)
                return Yinshi;
            else if (curHour == 5 || curHour == 6)
                return Moshi;
            else if (curHour == 7 || curHour == 8)
                return Chenshi;
            else if (curHour == 9 || curHour == 10)
                return Sishi;
            else if (curHour == 11 || curHour == 12)
                return Wushi;
            else if (curHour == 13 || curHour == 14)
                return Weishi;
            else if (curHour == 15 || curHour == 16)
                return Shenshi;
            else if (curHour == 17 || curHour == 18)
                return Youshi;
            else if (curHour == 19 || curHour == 20)
                return Shushi;
            else if (curHour == 21 || curHour == 22)
                return Haishi;
            else
                return Choshi;
        }

        //===============时间履历

        //===============时间事件

        /// <summary>
        /// 新增时间
        /// </summary>
        public void AddTimeEvent()
        {



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="startMonth"></param>
        /// <param name="startDay"></param>
        /// <param name="endYear"></param>
        /// <param name="endMonth"></param>
        /// <param name="endDay"></param>
        /// <param name="target"></param>
        public void AddTimeEvent(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, params object[] target)
        {



        }


        public void AddTimeEvent(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay, int eventId)
        {



        }

        /// <summary>
        /// 删除时间
        /// </summary>
        public void RemoveTimeEvent()
        {

        }

    }


}
