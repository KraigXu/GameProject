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
        private RectTransform _titlePrefab;

        private Dictionary<int, UiTitleitem>  _modelTitle=new Dictionary<int, UiTitleitem>();
        private int _idCounter;

        protected override void InitWindowData()
        {
            this.ID = WindowID.FixedTitleWindow;

            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;

        }

        public override void InitWindowOnAwake()
        {
        }

        /// <summary>
        /// 新增固定气泡
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeId"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public int AddTitle(ElementType type, int typeId, Vector3 point)
        {
            int id=0;
            switch (type)
            {
                case ElementType.LivingArea:
                    RectTransform rectGo = WXPoolManager.Pools[Define.PoolName].Spawn(_titlePrefab, transform);
                    UiTitleitem item = rectGo.GetComponent<UiTitleitem>();
                    item.Id = ++_idCounter;
                    item.Lable.text = GameStaticData.LivingAreaName[typeId];
                    item.Init(Camera.main,UICenterMasterManager.Instance._Camera,point);
                    id = item.Id;
                    break;
                default:
                    break;
            }
            return id;
        }

    }

}