﻿
namespace GameSystem.Ui
{
    public class BuildingWindow : UIWindowBase
    {
        public override void InitWindowOnAwake()
        {
            this.ID = WindowID.BuildingWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }

        protected override void InitWindowData()
        {

        }
    }

}