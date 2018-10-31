using System.Collections;
using System.Collections.Generic;
using LivingArea;
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
        public override void InitWindowOnAwake()
        {
            if (StrategySceneControl.Instance.M_Strategy.LivingAreas.Count > 0)
            {
                List<LivingAreaNode> nodes = StrategySceneControl.Instance.M_Strategy.LivingAreas;
                for (int i = 0; i < nodes.Count; i++)
                {
                    _titleItems[i].Init(nodes[i].transform);
                }
            }
        }
    }
}