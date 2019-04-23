
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem.Ui
{

    /// <summary>
    /// 根据Entity显示数据
    /// </summary>
    public class BuildingBazaarWindow : UIWindowBase
    {


        public Entity Entity;

        public EntityContentData EntityData;

        [SerializeField]
        private RectTransform _featureseParent;
        [SerializeField]
        private RectTransform _personParent;


        private RectTransform _viewmm;
        private RectTransform _viewyl;
        private RectTransform _viewdz;
        private RectTransform _viewfj;
        
        

        public override void InitWindowOnAwake()
        {
            this.ID = WindowID.BuildingBazaarWindow;

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

            _viewmm = (RectTransform)transform.Find("MM").transform;
            _viewmm.gameObject.SetActive(false);

            _viewyl = (RectTransform)transform.Find("YL").transform;
            _viewyl.gameObject.SetActive(false);

            _viewdz = (RectTransform)transform.Find("DZ").transform;
            _viewdz.gameObject.SetActive(false);

            _viewfj = (RectTransform) transform.Find("FJ").transform;
            _viewfj.gameObject.SetActive(false);


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
                return;
            }
            base.BeforeShowWindow(contextData);
            BuildingBlacksmithSystem blacksmithSystem = SystemManager.Get<BuildingBlacksmithSystem>();
            EntityData = (EntityContentData)contextData;

            BuildingBlacksmith buildingBlacksmith = SystemManager.GetProperty<BuildingBlacksmith>(EntityData.Entity);


            if (buildingBlacksmith.LevelId >= 0)
            {
                RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _featureseParent);
                UiBuildingItem uiItem = item.GetComponent<UiBuildingItem>();
                uiItem.Value = "买卖";
                uiItem.OnBuildingEnter = YL;
            }


            if (buildingBlacksmith.LevelId >= 1)
            {
                RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _featureseParent);
                UiBuildingItem uiItem = item.GetComponent<UiBuildingItem>();
                uiItem.Value = "冶炼";
                uiItem.OnBuildingEnter = YL;
            }

            if (buildingBlacksmith.LevelId >= 2)
            {
                RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _featureseParent);
                UiBuildingItem uiItem = item.GetComponent<UiBuildingItem>();
                uiItem.Value = "锻造";
                uiItem.OnBuildingEnter = DZ;

            }

            if (buildingBlacksmith.LevelId >= 3)
            {
                RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _featureseParent);
                UiBuildingItem uiItem = item.GetComponent<UiBuildingItem>();
                uiItem.Value = "分解";
                uiItem.OnBuildingEnter = FJ;
            }


            List<Entity> preson = SystemManager.Get<BehaviorSystem>().GetPositionCode(buildingBlacksmith.PositionCode);


            for (int i = 0; i < preson.Count; i++)
            {
                RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiPersonButton, _featureseParent);
                BiologicalBaseUi baseUi = item.GetComponent<BiologicalBaseUi>();
            }

        }

        public override void DestroyWindow()
        {
            base.DestroyWindow();

            for (int i = 0; i < _featureseParent.childCount; i++)
            {
                WXPoolManager.Pools[Define.GeneratedPool].Despawn(_featureseParent.GetChild(i));
            }

            for (int i = 0; i < _personParent.childCount; i++)
            {
                WXPoolManager.Pools[Define.GeneratedPool].Despawn(_personParent.GetChild(i));
            }
        }


        public void YL(Entity buildingEntity)
        {



        }

        public void DZ(Entity buildingEntity)
        {

        }

        public void FJ(Entity buildingEntity)
        {

        }



    }

}