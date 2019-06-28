using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem;
using Manager;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    /// <summary>
    /// 城市窗口
    /// </summary>
    public class CityWindow : UIWindowBase
    {

        [SerializeField] private Text _name;
        [SerializeField] private Text _powerName;
        [SerializeField] private Text _personName;
        [SerializeField] private Text _money;
        [SerializeField] private Text _iron;
        [SerializeField] private Text _wood;
        [SerializeField] private Text _food;
        [SerializeField] private Text _person;
        [SerializeField] private Text _level;
        [SerializeField] private Text _type;
        [SerializeField] private Text _stable;

        [SerializeField] private List<GameObject> _buildingBilling;
        [SerializeField] private List<Button> _buildingFeatures;
        [SerializeField] private List<BiologicalBaseUi> _buildingBiological;
        [SerializeField] private GameObject _buildingMainView;                                //建筑主视图
        [SerializeField] private GameObject _buildingImage;
        [SerializeField] private GameObject _buildingContent;

        [SerializeField]
        private RectTransform _billingParent;
        [SerializeField]
        private List<UiBuildingItem> _buildingItems = new List<UiBuildingItem>();
        [SerializeField]
        private Button _livingAreaExit;

        private bool _buildingIsShow = false;
        private LivingAreaWindowCD _livingAreaWindowCd;
        private LivingArea _livingArea;
        private LivingAreaData _livingAreaData;
        private float _changeCd;
        private Entity _laEntity;
        private EntityManager _entityManager;
        private LivingAreaSystem _livingAreaSystem;


        private Bounds _cityBounds;

        [Serializable]
        class TogglePanel
        {
            public Toggle Toggle;
            public RectTransform Panel;
        }

        protected override void InitWindowData()
        {
            this.ID = WindowID.CityWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }

        public override void InitWindowOnAwake()
        {
            _livingAreaExit.onClick.AddListener(CloseLivingArea);

            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
            _livingAreaSystem = SystemManager.Get<LivingAreaSystem>();
        }
        /// <summary>
        ///  //-----初始化选项
        /// </summary>
        /// <param name="contextData"></param>
        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {

            if (contextData == null)
            {
                Debuger.LogError("Enter LivingArea Error");
                return;
            }
            base.BeforeShowWindow(contextData);

            _livingAreaWindowCd = (LivingAreaWindowCD)contextData;
            _laEntity = _livingAreaWindowCd.LivingAreaEntity;
            _livingArea = SystemManager.GetProperty<LivingArea>(_livingAreaWindowCd.LivingAreaEntity);
            _livingAreaData = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", _livingArea.Id);

          //  _name.text = GameStaticData.LivingAreaName[_livingArea.Id];
            //_money.text = _livingArea.Money + "/" + _livingArea.MoneyMax;
            //_iron.text = _livingArea.Iron + "/" + _livingArea.IronMax;
            //_wood.text = _livingArea.Wood + "/" + _livingArea.WoodMax;
            //_food.text = _livingArea.Food + "/" + _livingArea.FoodMax;
            //_person.text = _livingArea.PersonNumber.ToString();
            //_stable.text = _livingArea.DefenseStrength.ToString();
            //_level.text = GameStaticData.LivingAreaLevel[_livingArea.CurLevel];
            //_type.text = GameStaticData.LivingAreaType[_livingArea.CurLevel];

            //Biological biological = _entityManager.GetComponentData<Biological>(entityBiological);
            //_powerName.text = GameStaticData.BiologicalSurnameDic[biological.BiologicalId];
            //_personName.text = GameStaticData.BiologicalNameDic[biological.BiologicalId];

            //初始化选项
            if (_buildingItems.Count > 0)
            {
                for (int i = 0; i < _buildingItems.Count; i++)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(_buildingItems[i].Rect);
                }
                _buildingItems.Clear();
            }

            //检查功能------------------>>>

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();


            if (entityManager.HasComponent<BuildingBazaar>(_laEntity))
            {
                BuildingBazaar buildingBazaar = entityManager.GetComponentData<BuildingBazaar>(_laEntity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _billingParent).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value = "市集";
                uiBuildingItem.BuildingEntity = _laEntity;
                uiBuildingItem.OnBuildingEnter = OpenBazaarWindow;

                _buildingItems.Add(uiBuildingItem);
            }


            if (entityManager.HasComponent<BuildingBlacksmith>(_laEntity))
            {
                BuildingBlacksmith buildingBlacksmith = entityManager.GetComponentData<BuildingBlacksmith>(_laEntity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _billingParent).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value = "TTT";
                uiBuildingItem.BuildingEntity = _laEntity;
                uiBuildingItem.OnBuildingEnter = OpenBlacksmithWindow;
                _buildingItems.Add(uiBuildingItem);
            }

            if (entityManager.HasComponent<BuildingTailor>(_laEntity))
            {
                BuildingTailor buildingTailor = entityManager.GetComponentData<BuildingTailor>(_laEntity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _billingParent).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value = "";
                uiBuildingItem.BuildingEntity = _laEntity;
                uiBuildingItem.OnBuildingEnter = null;  

                _buildingItems.Add(uiBuildingItem);
            }

            if (entityManager.HasComponent<BuidingTavern>(_laEntity))
            {
                BuidingTavern buidingTavern = entityManager.GetComponentData<BuidingTavern>(_laEntity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _billingParent).GetComponent<UiBuildingItem>();
                uiBuildingItem.Value = "JD";
                uiBuildingItem.BuildingEntity = _laEntity;
                uiBuildingItem.OnBuildingEnter = null;

                _buildingItems.Add(uiBuildingItem);
            }

          

            //检查功能------------------------------------<<<
           
            //Renderer[] renderers = StrategyStyle.Instance.ModelCityO1.GetComponentsInChildren<Renderer>();
            //_cityBounds = renderers[0].bounds;
            //for (int i = 1; i < renderers.Length; i++)
            //{
            //    _cityBounds.Encapsulate(renderers[i].bounds);
            //}

            StrategyScene.Instance.FixedCamera.enabled = true;
           // GameSceneInit.Settings.FixedCamera.enabled = true;
            //StrategyCameraManager.Instance.SetTarget(_cityBounds.center, true);
            // StrategyCameraManager.Instance.SetTarget(StrategyStyle.Instance.ModelCityO1.transform);
            // SystemManager.Get<PlayerControlSystem>().Target(bounds.center);

            //List<UiBuildingItem> buildingItems=SystemManager.Get<LivingAreaSystem>()
            //BuildingJsonData jsonData = JsonConvert.DeserializeObject<BuildingJsonData>(_livingAreaData.BuildingInfoJson);
            //List<Entity> entitys = _livingAreaSystem.GetBuilding(_livingAreaWindowCd.LivingAreaEntity);
            //for (int i = 0; i < entitys.Count; i++)
            //{
            //    UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_billingPrefab, _billingParent).GetComponent<UiBuildingItem>();
            //    HousesControl houses = _entityManager.GetComponentData<HousesControl>(entitys[i]);
            //    uiBuildingItem.BuildingEntity = entitys[i];

            //    BuildingItem item = jsonData.GetBuildingItem(houses.SeedId);
            //    uiBuildingItem.Name.text = item.BuildingName;

            //    UIEventTriggerListener.Get(uiBuildingItem.gameObject).onClick += AccessBuilding;
            //    _buildingItems.Add(uiBuildingItem);
            //}
            //Entity entityLivingArea = SystemManager.Get<LivingAreaSystem>().GetLivingAreaEntity(_livingAreaWindowCd.LivingAreaId);
            //LivingArea livingArea = _entityManager.GetComponentData<LivingArea>(entityLivingArea);

            //for (int i = 0; i < _buildingBilling.Count; i++)
            //{
            //    _buildingBilling[i].gameObject.SetActive(false);
            //}


            //Entity entityBiological = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(livingArea.PowerId);




            //List<Entity> entitieBuilding = SystemManager.Get<BuildingSystem>().GetBuildingGroup(livingArea.Id);

            //for (int i = 0; i < entitieBuilding.Count; i++)
            //{
            //    Building building = _entityManager.GetComponentData<Building>(entitieBuilding[i]);
            //    _buildingBilling[i].gameObject.SetActive(true);
            //    _buildingBilling[i].GetComponentInChildren<Text>().text = GameStaticData.BuildingName[building.BuildingModelId];
            //}

        }

        void OnDrawGizmos()
        {
            Gizmos.color=new Color(0,0,1,0.3f);
            Gizmos.DrawCube(_cityBounds.center, _cityBounds.size);

        }

        private void OpenBazaarWindow(Entity entity)
        {
            ShowWindowData showWindow=new ShowWindowData();
            EntityContentData entityWindowData=new EntityContentData();
            entityWindowData.Entity = entity;
            showWindow.contextData = entityWindowData;

            UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingBazaarWindow, showWindow);

        }


        private void OpenBlacksmithWindow(Entity entity)
        {
            ShowWindowData showWindow = new ShowWindowData();
            EntityContentData entityWindowData = new EntityContentData();
            entityWindowData.Entity = entity;
            showWindow.contextData = entityWindowData;

            UICenterMasterManager.Instance.ShowWindow(WindowID.BuildingBlacksmithWindow, showWindow);
        }


        public void BuildingEnter(Entity entity)
        {




        }


        private void ChangeData()
        {

            LivingArea livingArea = SystemManager.GetProperty<LivingArea>(_livingAreaWindowCd.LivingAreaEntity);
            for (int i = 0; i < _buildingBilling.Count; i++)
            {
                _buildingBilling[i].gameObject.SetActive(false);
            }

            _name.text = GameStaticData.LivingAreaName[livingArea.Id];
            _money.text = livingArea.Money + "/" + livingArea.MoneyMax;
            _iron.text = livingArea.Iron + "/" + livingArea.IronMax;
            _wood.text = livingArea.Wood + "/" + livingArea.WoodMax;
            _food.text = livingArea.Food + "/" + livingArea.FoodMax;
            _person.text = livingArea.PersonNumber.ToString();
            _stable.text = livingArea.DefenseStrength.ToString();
            _level.text = livingArea.CurLevel.ToString();
            _type.text = livingArea.Type.ToString();


        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="go"></param>
        private void AccessBuilding(GameObject go)
        {
            UiBuildingItem uiBuildingItem = go.GetComponent<UiBuildingItem>();

            // SystemManager.Get<BuildingSystem>().ShowBuildingInside(uiBuildingItem.BuildingEntity, StrategySceneInit.PlayerEntity, _livingAreaEntity);
            //BuildingSystem.ShowBuildingInside(uiBuildingItem.BuildingEntity, StrategySceneInit.PlayerEntity, _livingAreaEntity);
            return;
            BuildingiDataItem item = null;
            for (int i = 0; i < _buildingBilling.Count; i++)
            {
                if (go == _buildingBilling[i].gameObject)
                {
                    item = _livingAreaWindowCd.BuildingiDataItems[i];
                }
            }

            if (item == null)
                return;

            _buildingIsShow = true;


            //for (int i = 0; i < item.Features.Count; i++)
            //{
            //    _buildingFeatures[i].GetComponentInChildren<Text>().text = GameStaticData.FeaturesName[item.Features[i].Id];
            //}

            //if (item.Biologicals.Count > _buildingBiological.Count)        //如果长度不够，则补齐数据
            //{
            //    int number = item.Biologicals.Count - _buildingBiological.Count;

            //    for (int i = 0; i < number; i++)
            //    {
            //        GameObject newItem = UGUITools.AddChild(_buildingBiological[0].transform.parent.gameObject, _buildingBiological[0].gameObject);
            //        _buildingBiological.Add(newItem.GetComponent<BiologicalBaseUi>());
            //    }
            //}

            //for (int i = 0; i < item.Biologicals.Count; i++)
            //{
            //    _buildingBiological[i].NameTex.text = item.Biologicals[i].Name;
            //    _buildingBiological[i].HeadImg.sprite = GameStaticData.BiologicalAvatar[item.Biologicals[i].AtlasId];
            //    _buildingBiological[i].gameObject.name = item.Biologicals[i].Id.ToString();
            //}
            item.OnOpen?.Invoke(item.OnlyEntity, item.Id);
        }

        /// <summary>
        /// 退出
        /// </summary>
        private void Exit()
        {
            for (int i = 0; i < _buildingItems.Count; i++)
            {
                WXPoolManager.Pools[Define.GeneratedPool].Despawn(_buildingItems[i].transform);
            }
            _buildingItems.Clear();

            this.CloseWindow();
        }

        private void CloseBuidingView()
        {
            _buildingIsShow = false;
        }

        private void CloseLivingArea()
        {

        }

        void OnUpdate()
        {
            //_changeCd += Time.deltaTime;
            //if (_changeCd > 1)
            //{
            //    _changeCd = 0;
            //    ChangeData();
            //}
        }

    }
}