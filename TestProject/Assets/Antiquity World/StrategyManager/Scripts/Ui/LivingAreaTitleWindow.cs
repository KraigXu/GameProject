using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Ui
{

    /// <summary>
    /// 城市标题窗口
    /// </summary>
    public class LivingAreaTitleWindow : UIWindowBase
    {
        protected override void InitWindowData()
        {
            this.ID = WindowID.FixedTitleWindow;

            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;

        }

        private RectTransform _Prefab;

        private Dictionary<int, RectTransform> _titleDisplaysMap = new Dictionary<int, RectTransform>();
        private Dictionary<int, RectTransform> _titleHideMap = new Dictionary<int, RectTransform>();


        public Dictionary<int, RectTransform> TitleDisplayMap
        {
            get { return _titleDisplaysMap; }
        }

        public Dictionary<int, RectTransform> TitleHideMap
        {
            get { return _titleHideMap; }
        }

        public override void InitWindowOnAwake()
        {
        }

        public void ShowWindow(int id, Transform node)
        {
           // RectTransform titleRtf = WXPoolManager.Pools[Define.ParticlePool].Spawn(_Prefab, transform);
          //  titleRtf.gameObject.GetComponent<>()
        }

    }

}