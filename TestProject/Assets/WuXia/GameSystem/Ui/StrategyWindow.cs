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

        protected override void SetWindowId()
        {
            this.ID = WindowID.StrategyWindow;
        }

        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.ForegroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }

        public override void InitWindowOnAwake()
        {
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            base.BeforeShowWindow(contextData);
            if (contextData != null)
            {
                StrategyWindowInData data = (StrategyWindowInData) contextData;
                //_characterInformationBtn.onClick.AddListener(data.CharavterEvent);
                //_wugongBtn.onClick.AddListener(data.WugongEvent);
                //_technologyBtn.onClick.AddListener(data.TechnologyEvent);
               // _logBtn.onClick.AddListener(data.LogEvent);
               // _mapBtn.onClick.AddListener(data.MapEvent);

               // AvatarImage.overrideSprite = GameStaticData.BiologicalAvatar[data.PlayerAvatarId];
               // NameTex.text = GameStaticData.BiologicalNameDic[data.PlayerId];
               // SunNameTex.text = GameStaticData.BiologicalSurnameDic[data.PlayerId];
            }

            

        }


        public void UpdateTime(TimeData timeData)
        {
            this.Year.text = timeData.Year.ToString();
            this.Month.text = timeData.Month.ToString();
            this.Day.text = timeData.Day.ToString();
            this.ShiChen.text = GameStaticData.TimeShichen[timeData.Shichen];
            this.Season.text = GameStaticData.TimeJijie[timeData.Jijie];

        }
    }
}