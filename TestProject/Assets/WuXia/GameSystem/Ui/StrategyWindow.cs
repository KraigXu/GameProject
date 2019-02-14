﻿using System;
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
    /// 主UI
    /// </summary>
    public class StrategyWindow : UIWindowBase
    {

        public Text Year;
        public Text Month;
        public Text Day;
        public Text ShiChen;
        public Text Season;

        private float _cd;

        protected override void InitWindowData()
        {
            this.ID = WindowID.StrategyWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;

        }


        public override void InitWindowOnAwake()
        {
        }


        void Update()
        {
            UpdateTime();
        }


        public void UpdateTime()
        {
            _cd += Time.deltaTime;
            if (_cd > 1)
            {
                this.Year.text = WorldTimeSystem.Year.ToString();
                this.Month.text = WorldTimeSystem.Month.ToString();
                this.Day.text = WorldTimeSystem.Day.ToString();

                this.ShiChen.text = GameStaticData.TimeShichen[WorldTimeSystem.Shichen];
                this.Season.text = GameStaticData.TimeJijie[WorldTimeSystem.Jijie];
                _cd = 0;
            }
        }
    }
}