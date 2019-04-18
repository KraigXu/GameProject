using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem;
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
            _livingArea = SystemManager.GetProperty<LivingArea>(_livingAreaWindowCd.LivingAreaEntity);
            _livingAreaData = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", _livingArea.Id);


            //初始化选项
            if (_buildingItems.Count > 0)
            {
                for (int i = 0; i < _buildingItems.Count; i++)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(_buildingItems[i].Rect);
                }
                _buildingItems.Clear();
            }



            //检查功能----------------------------------->>>

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            if (entityManager.HasComponent<BuildingBlacksmith>(_laEntity))
            {
                BuildingBlacksmith buildingBlacksmith = entityManager.GetComponentData<BuildingBlacksmith>(_laEntity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _billingParent).GetComponent<UiBuildingItem>();
                uiBuildingItem.Name.text = "TTT";
               // uiBuildingItem
                //uiBuildingItem.OnBuildingEnter = OpenUi;

            }

            if (entityManager.HasComponent<BuildingBazaar>(_laEntity))
            {
                BuildingBazaar buildingBazaar = entityManager.GetComponentData<BuildingBazaar>(_laEntity);
                UiBuildingItem uiBuildingItem = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiFunctionButton, _billingParent).GetComponent<UiBuildingItem>();
                uiBuildingItem.Name.text = "SJ";


            }





            //if (SystemManager.Contains<BuildingBlacksmith>(_livingAreaWindowCd.LivingAreaEntity))
            //{
            //    BuildingBlacksmith buildingBlacksmith = SystemManager.GetProperty<BuildingBlacksmith>(entity);


            //}



            //检查功能------------------------------------<<<
           
            
           




            for (int i = 0; i < StrategySceneInit.BuildingSystems.Count; i++)
            {

                var item = StrategySceneInit.BuildingSystems[i];

                //if (item.GetBuildingItem(_livingAreaEntity))
                //{

                //}


                UiBuildingItem buildingitem = item.GetBuildingItem(_livingAreaWindowCd.LivingAreaEntity);
                if (buildingitem != null)
                {
                    buildingitem.Rect.SetParent(_billingParent);
                    _buildingItems.Add(buildingitem);
                }

            }


            Renderer[] renderers = StrategyStyle.Instance.ModelCityO1.GetComponentsInChildren<Renderer>();
            Bounds bounds = renderers[0].bounds;
            for (int i = 0; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            SystemManager.Get<PlayerControlSystem>().Target(bounds.center);
            return;

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
            //_name.text = GameStaticData.LivingAreaName[livingArea.Id];
            //_money.text = livingArea.Money + "/" + livingArea.MoneyMax;
            //_iron.text = livingArea.Iron + "/" + livingArea.IronMax;
            //_wood.text = livingArea.Wood + "/" + livingArea.WoodMax;
            //_food.text = livingArea.Food + "/" + livingArea.FoodMax;
            //_person.text = livingArea.PersonNumber.ToString();
            //_stable.text = livingArea.DefenseStrength.ToString();
            //_level.text = GameStaticData.LivingAreaLevel[livingArea.CurLevel];
            //_type.text = GameStaticData.LivingAreaType[livingArea.TypeId];

            //Entity entityBiological = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(livingArea.PowerId);
            //Biological biological = _entityManager.GetComponentData<Biological>(entityBiological);

            //_powerName.text = GameStaticData.BiologicalSurnameDic[biological.BiologicalId];
            //_personName.text = GameStaticData.BiologicalNameDic[biological.BiologicalId];

            //List<Entity> entitieBuilding = SystemManager.Get<BuildingSystem>().GetBuildingGroup(livingArea.Id);

            //for (int i = 0; i < entitieBuilding.Count; i++)
            //{
            //    Building building = _entityManager.GetComponentData<Building>(entitieBuilding[i]);
            //    _buildingBilling[i].gameObject.SetActive(true);
            //    _buildingBilling[i].GetComponentInChildren<Text>().text = GameStaticData.BuildingName[building.BuildingModelId];
            //}

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