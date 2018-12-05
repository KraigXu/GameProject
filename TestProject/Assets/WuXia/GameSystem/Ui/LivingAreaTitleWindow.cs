using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameSystem.Ui
{
    public class LivingAreaTitleWindow : UIWindowBase
    {
        [SerializeField]
        private List<LivingAreaTitleItem> _titleItems = new List<LivingAreaTitleItem>();
        protected override void SetWindowId()
        {
            this.ID = WindowID.LivingAreaTitleWindow;
        }

        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.Scale;
        }
        public override void InitWindowOnAwake(){}

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            if (contextData != null)
            {
                WindowContextLivingAreaData data = contextData as WindowContextLivingAreaData;
                for (int i = 0; i < data.EntityArray.Count; i++)
                {
                    _titleItems[i].Init(GameStaticData.LivingAreaName[data.EntityArray[i]],Camera.main,UICenterMasterManager.Instance._Camera, data.Points[i]+ new Vector3(0,1f,0));
                }
                
            }
        }
    }
}