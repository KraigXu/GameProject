using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Ui
{
    /// <summary>
    /// 固定标题窗口
    /// </summary>
    public class FixedTitleWindow : UIWindowBase
    {
        [SerializeField]
        private List<LivingAreaTitleItem> _titleItems = new List<LivingAreaTitleItem>();

        [SerializeField]
        private Dictionary<int, LivingAreaTitleItem> _itemDic=new Dictionary<int, LivingAreaTitleItem>();

        protected override void SetWindowId()
        {
            this.ID = WindowID.FixedTitleWindow;
        }

        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
      
        }
        public override void InitWindowOnAwake() { }

        //public override void ShowWindow(BaseWindowContextData contextData)
        //{
        //    if (contextData != null)
        //    {
        //        WindowContextLivingAreaData data = contextData as WindowContextLivingAreaData;
        //        for (int i = 0; i < data.EntityArray.Count; i++)
        //        {
        //            _titleItems[i].Init(GameStaticData.LivingAreaName[data.EntityArray[i]], Camera.main, UICenterMasterManager.Instance._Camera, data.Points[i] + new Vector3(0, 1f, 0));
        //        }

        //    }
        //}


        public void Add(int id)
        {
            if (_itemDic.ContainsKey(id) == false)
            {

            }
            else
            {

            }


        }
    }

}