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
    /// 战略场景主UI 
    /// </summary>
    public class StrategyWindow : UIWindowBase
    {

        public Button settingButton;
        public GameObject settingView;


        public Button relationMapButton;
        public Button mapButton;
        public Button illustrationButton;

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
            settingButton.onClick.AddListener(SettingClick);

            //Define
        }


        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                settingView.SetActive(!settingView.activeSelf);

            }


        }

        public void SettingClick()
        {
            
        }


        


        

    }
}