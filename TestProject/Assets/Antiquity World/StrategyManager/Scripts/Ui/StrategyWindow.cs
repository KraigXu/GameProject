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
    /// 
    /// </summary>
    public class StrategyWindow : UIWindowBase
    {


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

    }
}