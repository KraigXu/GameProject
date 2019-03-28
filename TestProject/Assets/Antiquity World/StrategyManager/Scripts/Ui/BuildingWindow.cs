
using UnityEngine;

namespace GameSystem.Ui
{
    public class BuildingWindow : UIWindowBase
    {
        [SerializeField]
        private RectTransform _featurePrefab;


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

        /// <summary>
        /// 在显示前初始化数据
        /// </summary>
        /// <param name="contextData"></param>
        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null)
            {
                Debug.LogError("房屋信息为NULL");
            }
            base.BeforeShowWindow(contextData);


        }



    }

}