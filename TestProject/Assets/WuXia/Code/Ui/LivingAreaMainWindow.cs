using System;
using System.Collections;
using System.Collections.Generic;
using WX;
using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
{
    /// <summary>
    /// LivingArea主视图
    /// </summary>
    public class LivingAreaMainWindow : UIWindowBase
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
        [SerializeField] private List<GameObject> _buildingFeatures;
        [SerializeField] private List<BiologicalBaseUi> _buildingBiological;
        [SerializeField] private GameObject _buildingMainView;                                //建筑主视图
        [SerializeField] private GameObject _buildingImage;
        [SerializeField] private GameObject _buildingContent;

       // [SerializeField] private List<TogglePanel> _buildingViewGroup;                         //管理建筑物组

        [SerializeField] private Button _livingAreaExit;
        [SerializeField] private Button _buildingExit;

        private bool _buildingIsShow = false;

        private LivingAreaWindowCD _currentLivingArea;

        [Serializable]
        class TogglePanel
        {
            public Toggle Toggle;
            public RectTransform Panel;
        }

        protected override void SetWindowId()
        {
            this.ID = WindowID.LivingAreaMainWindow;
        }

        protected override void InitWindowCoreData()
        {
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
            _buildingExit.onClick.AddListener(CloseBuidingView);
            _livingAreaExit.onClick.AddListener(CloseLivingArea);
        }

        void Update()
        {
            _buildingMainView.gameObject.SetActive(_buildingIsShow);
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null) return;
            _currentLivingArea = (LivingAreaWindowCD)contextData;

            ChangeData();
        }

        private void ChangeData()
        {
            for (int i = 0; i < _buildingBilling.Count; i++)
            {
                _buildingBilling[i].gameObject.SetActive(false);
            }
            _name.text = GameStaticData.LivingAreaName[_currentLivingArea.LivingAreaId];
            _powerName.text = GameStaticData.LivingAreaName[_currentLivingArea.LivingAreaId];
            _personName.text = GameStaticData.LivingAreaName[_currentLivingArea.LivingAreaId];
            _money.text = _currentLivingArea.Money + "/" + _currentLivingArea.MoneyMax;
            _iron.text = _currentLivingArea.Iron + "/" + _currentLivingArea.IronMax;
            _wood.text = _currentLivingArea.Wood + "/" + _currentLivingArea.WoodMax;
            _food.text = _currentLivingArea.Food + "/" + _currentLivingArea.FoodMax;
            _person.text = _currentLivingArea.PersonNumber.ToString();
            _stable.text = _currentLivingArea.DefenseStrength.ToString();
            _level.text = GameStaticData.LivingAreaLevel[_currentLivingArea.LivingAreaLevel];
            _type.text = GameStaticData.LivingAreaType[_currentLivingArea.LivingAreaType];

            for (int i = 0; i < _currentLivingArea.BuildingiDataItems.Count; i++)
            {
                _buildingBilling[i].gameObject.SetActive(true);
                _buildingBilling[i].GetComponentInChildren<Text>().text = GameStaticData.BuildingName[_currentLivingArea.BuildingiDataItems[i].Id];
               //_buildingBilling[i].Init(StrategySceneInit.Settings.MainCamera, UICenterMasterManager.Instance._Camera,_currentLivingArea.BuildingiDataItems[i].Point);
               
                UIEventTriggerListener.Get(_buildingBilling[i].gameObject).onClick += AccessBuilding;
            }
        }

        private void AccessBuilding(GameObject go)
        {
            BuildingiDataItem item = null;
            for (int i = 0; i < _buildingBilling.Count; i++)
            {
                if (go == _buildingBilling[i].gameObject)
                {
                    item = _currentLivingArea.BuildingiDataItems[i];
                }
            }

            if(item==null)
                return;

            _buildingIsShow = true;


            if (item.Biologicals.Count > _buildingBiological.Count)        //如果长度不够，则补齐数据
            {
                int number = item.Biologicals.Count - _buildingBiological.Count;

                for (int i = 0; i < number; i++)
                {
                    GameObject newItem= UGUITools.AddChild(_buildingBiological[0].transform.parent.gameObject, _buildingBiological[0].gameObject);
                    _buildingBiological.Add(newItem.GetComponent<BiologicalBaseUi>());
                }
            }

            for (int i = 0; i < item.Biologicals.Count; i++)
            {
                _buildingBiological[i].NameTex.text = item.Biologicals[i].Name;
                _buildingBiological[i].HeadImg.sprite = GameStaticData.BiologicalAvatar[item.Biologicals[i].AtlasId];
                _buildingBiological[i].gameObject.name = item.Biologicals[i].Id.ToString();
            }
            item.OnOpen?.Invoke(item.OnlyEntity, item.Id);
        }

        private void OpenBuidingView(BuildingObject building)
        {
            _buildingIsShow = true;
        }

        private void CloseBuidingView()
        {
            _buildingIsShow = false;
        }

        private void CloseLivingArea()
        {
           
        }

    }
}