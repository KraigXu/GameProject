using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

namespace GameSystem.Ui
{
    /// <summary>
    /// 世界时间UI
    /// </summary>
    public class WorldTimeWindow : UIWindowBase
    {
        public Text Year;
        public Text Month;

        public Text ShiChen;
        public Text Season;
        public Slider SliderDay;
        private float _cd;


        private Dictionary<int, string> TimeJijie = new Dictionary<int, string>();

        private Dictionary<int, string> TimeShichen = new Dictionary<int, string>();

        protected override void InitWindowData()
        {
            this.ID = WindowID.WorldTimeWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        public override void InitWindowOnAwake()
        {
            //        子时 丑时  寅时 卯时  辰时 巳时
            //        23:00 - 00:59 01:00 - 02:59 03:00 - 04:59 05:00 - 06:59 07:00 - 08:59 09:00 - 10:59
            //        午时 未时  申时 酉时  戊时 亥时
            //        11:00 - 12:59 13:00 - 14:59 15:00 - 16:59 17:00 - 18:59 19:00 - 20:59 21:00 - 22:59

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
        }


        public void AddFloat(int year,int month,int season,float dayvalue)
        {
            Year.text = year.ToString();
            Month.text = month.ToString();

            SliderDay.value = dayvalue;

            //_worldTimeWindow.Year.text = Ye

            //_cd += Time.deltaTime;
            //if (_cd > 1)
            //{
            //    Year.text = WorldTime.Year.ToString();
            //    Month.text = WorldTime.Month.ToString();
            //    Day.text = WorldTime.Day.ToString();
            //    ShiChen.text = TimeShichen[WorldTime.ShiChen];
            //    //Season.text = _timeManager.Season;
            //    _cd = 0;
            //}
            //SliderDay.valu
        }


    }
}