using WX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyFrameWork
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
                string[] names = data.Names;
                Vector3[] point = data.Points;
                for (int i = 0; i < names.Length; i++)
                {
                    _titleItems[i].Init(names[i],Camera.main,UICenterMasterManager.Instance._Camera,point[i]+new Vector3(0,1f,0));
                }
                
            }
        }
    }
}