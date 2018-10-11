using System.Collections;
using System.Collections.Generic;
using LivingArea;
using UnityEngine;
using UnityEngine.UI;

namespace TinyFrameWork
{
    public class LivingAreaBasicWindow : UIWindowBase
    {
        public Text CityNameText;
        public Text CityLevelText;
        public Text CityModenyText;
        public Text CityPowerText;
        public Text CityHaveText;
        public Text CityDescriptionText;
        
        protected override void SetWindowId()
        {
            this.ID = WindowID.LivingAreaBasicWindow;
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
            WindowContextLivingAreaNodeData data = (WindowContextLivingAreaNodeData)contextData;
            if (data != null)
            {
                LivingAreaNode node = data.Node;
                CityNameText.text = node.LivingAreaName;
                CityLevelText.text = node.BuildingLevel.ToString();
                CityModenyText.text = node.LivingAreaMoney.ToString();
                CityPowerText.text=node.PowerId.ToString();
                CityHaveText.text=node.HaveId.ToString();
                CityDescriptionText.text=node.Description;

            }
        }

        void Update()
        {

        }


        void OnDisable()
        {
            
        }


    }

}
