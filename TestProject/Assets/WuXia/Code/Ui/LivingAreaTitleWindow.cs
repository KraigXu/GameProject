using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyFrameWork
{
    public class LivingAreaTitleWindow : UIWindowBase
    {

        private GameObject _titleItemPrefab;
        private List<LivingAreaTitleItem> _titleItems;
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
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }

        public override void InitWindowOnAwake()
        {
            _titleItemPrefab = Resources.Load<GameObject>("UiPrefab/LivingArea/LivingAreaTitleItem");
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            WindowContextLivingAreaData livingAreaData = (WindowContextLivingAreaData)contextData;
            _titleItems = new List<LivingAreaTitleItem>();
            for (int i = 0; i < livingAreaData.Nodes.Count; i++)
            {
                LivingAreaTitleItem titleItem = UGUITools.AddChild(gameObject, _titleItemPrefab).GetComponent<LivingAreaTitleItem>();
                titleItem.Init( livingAreaData.Nodes[i].transform);
                _titleItems.Add(titleItem);
            }

        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}