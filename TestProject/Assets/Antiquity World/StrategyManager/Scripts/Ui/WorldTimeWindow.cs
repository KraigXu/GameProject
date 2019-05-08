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
        public Text Day;
        public Text ShiChen;
        public Text Season;

        private float _cd;
        private WorldTimeManager _timeManager;

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
            _timeManager = WorldTimeManager.Instance;
        }


        void Update()
        {
            _cd += Time.deltaTime;
            if (_cd > 1)
            {
                Year.text = _timeManager.Year.ToString();
                Month.text = _timeManager.Month.ToString();
                Day.text = _timeManager.Day.ToString();
                ShiChen.text = _timeManager.ShiChen;
                Season.text = _timeManager.Season;
                _cd = 0;
            }

        }




    }
}