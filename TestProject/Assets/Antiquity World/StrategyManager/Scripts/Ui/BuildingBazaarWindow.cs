
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{

    /// <summary>
    /// 根据Entity显示数据
    /// </summary>
    public class BuildingBazaarWindow : UIWindowBase
    {
        public EntityContentData EntityData;
        [SerializeField]
        private RectTransform _featureseParent;
        [SerializeField]
        private RectTransform _personParent;

        public RectTransform BuyingView;

        public RectTransform CurView;


        public override void InitWindowOnAwake()
        {
            this.ID = WindowID.BuildingBazaarWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
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
                return;
            }
            base.BeforeShowWindow(contextData);
            EntityData = (EntityContentData)contextData;
            BuildingBazaarSystem blacksmithSystem = SystemManager.Get<BuildingBazaarSystem>();

            BuildingBazaar bazaar = SystemManager.GetProperty<BuildingBazaar>(EntityData.Entity);

            {
                RectTransform item=WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton,_featureseParent);
                UiBuildingItem buildingItem= item.GetComponent<UiBuildingItem>();
                buildingItem.Value = "XXXX";
                buildingItem.OnBuildingEnter = Dealer;
            }

            {
                RectTransform item=WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton,_featureseParent);
                UiBuildingItem buildingItem=item.GetComponent<UiBuildingItem>();
                buildingItem.Value = "XXXX";
                buildingItem.OnBuildingEnter = Dealer;
            }



            {   //显示人物信息
                List<Entity> entities = SystemManager.Get<BehaviorSystem>().GetPositionCode(bazaar.PositionCode);
                for (int i = 0; i < entities.Count; i++)
                {
                    RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.UiPersonButton, _featureseParent);
                    BiologicalBaseUi biologicalUi = item.GetComponent<BiologicalBaseUi>();
                    biologicalUi.Entity = entities[i];
                }
            }
        }

        public void Dealer(Entity buildingEntity)
        {
            if (CurView != null)
            {
                CurView.gameObject.SetActive(false);
            }

            BuyingView.gameObject.SetActive(true);
            CurView = BuyingView;

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

    }

}