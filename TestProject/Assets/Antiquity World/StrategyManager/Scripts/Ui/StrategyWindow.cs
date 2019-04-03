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
                Year.text = WorldTimeManager.Instance.Year.ToString();
                Month.text = WorldTimeManager.Instance.Month.ToString();
                Day.text = WorldTimeManager.Instance.Day.ToString();
               // ShiChen.text=   GameStaticData.TimeShichen[WorldTimeManager.Instance.Shichen];
              //  Season.text = GameStaticData.TimeJijie[WorldTimeManager.Instance.Jijie];
                _cd = 0;
            }
        }
    }
}