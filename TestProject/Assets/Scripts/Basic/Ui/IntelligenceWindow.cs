using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    /// <summary>
    /// 情报窗口
    /// </summary>
    public class IntelligenceWindow : UIWindowBase
    {
        [SerializeField]
        private Toggle _personInfoTog;
        [SerializeField]
        private GameObject _personInfoGo;
        [SerializeField]
        private Toggle _factionTog;
        [SerializeField]
        private GameObject _factionGo;
        [SerializeField]
        private Toggle _familyTog;
        [SerializeField]
        private GameObject _familyGo;
        [SerializeField]
        private Toggle _wuxueTog;
        [SerializeField]
        private GameObject _wuxueGo;
        [SerializeField]
        private Toggle _livingAreaTog;
        [SerializeField]
        private GameObject _livingAreaGo;

        [SerializeField]
        private RectTransform _listItemPrefab;
        [SerializeField]
        private List<UiListItem> _listItems = new List<UiListItem>();
        [SerializeField]
        private RectTransform _listItemParent;

        [SerializeField]
        private Button _exitBtn;

        [Header("PersonInfo")]
        public Image PonAvatr;
        public Text PonName;
        public Text PonSex;
        public Text PonPrestige;
        public Text PonFamily;
        public Toggle PonInfoTog1;
        public Toggle PonInfoTog2;
        public Toggle PonInfoTog3;
        public Toggle PonInfoTog4;

        [Header("Faction")]
        public Text FactionName;
        public Text FactionDescription;

        [Header("LivingAreaInfo")]
        public Transform LivingAreaContent;
        public Toggle StatusTog;             
        public RectTransform StatusView;
        public Toggle BulidingTog;              
        public RectTransform BulidingView;
        public Toggle AnnualHistoryTog;              
        public RectTransform AnnualHistoryView;

        protected override void InitWindowData()
        {
            this.ID = WindowID.IntelligenceWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
            windowData.animationType = UIWindowAnimationType.FadesOut;
        }
        
        public override void InitWindowOnAwake()
        {
            _exitBtn.onClick.AddListener(delegate ()
            {
                UICenterMasterManager.Instance.DestroyWindow(this.ID);
            });

            _personInfoTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _personInfoGo.SetActive(flag);
                if (flag)
                    ChangePersonInfo();
            });

            _factionTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _factionGo.SetActive(flag);
                if (flag)
                    ChangeFactionInfo();
            });

            _familyTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _familyGo.SetActive(flag);
            });

            _wuxueTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _wuxueGo.SetActive(flag);
                if (flag)
                {
                    ChangeWuxueInfo();
                }
            });

            _livingAreaTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _livingAreaGo.SetActive(flag);
                if (flag)
                {
                    ChangeLivingAreaInfo();
                }
            });
            StatusTog.onValueChanged.AddListener(delegate(bool flag)
            {
                StatusView.gameObject.SetActive(flag);
            });

            BulidingTog.onValueChanged.AddListener(delegate(bool flag)
            {
                BulidingView.gameObject.SetActive(flag);
            });

            AnnualHistoryTog.onValueChanged.AddListener(delegate(bool flag)
            {
                AnnualHistoryView.gameObject.SetActive(flag);
            });
            StatusTog.isOn = true;
        }

        private void ClearListItem()
        {
            for (int i = 0; i < _listItems.Count; i++)
            {
                _listItems[i].Destroy();
            }
            _listItems.Clear();
        }

        private void ChangePersonInfo()
        {
            ClearListItem();
            //List<int> biologicalIds = World.Active.GetExistingManager<BiologicalSystem>().GetAllBiologicalName();

            //for (int i = 0; i < biologicalIds.Count; i++)
            //{
            //    RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_listItemPrefab, _listItemParent);

            //    UiListItem uiitem = item.GetComponent<UiListItem>();
            //  //  uiitem.Text.text = GameStaticData.BiologicalDictionary[biologicalIds[i]] + GameStaticData.BiologicalDictionary[biologicalIds[i]];
            //    uiitem.Id = biologicalIds[i];
            //    uiitem.ClickCallback = ShowPersonDetailedInfo;
            //    _listItems.Add(uiitem);
            //}
        }

        private void ShowPersonDetailedInfo(UiListItem item)
        {
            Entity entity= SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(1);
            Biological biological = SystemManager.GetProperty<Biological>(entity);
            PonAvatr.sprite = StrategyAssetManager.GetBiologicalAvatar(biological.AvatarId);
         //   PonName.text = GameStaticData.BiologicalDictionary[biological.BiologicalId] + GameStaticData.BiologicalDictionary[biological.BiologicalId];
         //  PonSex.text = GameStaticData.BiologicalDictionary[biological.Sex];
           // PonPrestige.text = World.Active.GetExistingManager<PrestigeSystem>().CheckValue(biological.PrestigeValue, ElementType.Biological);
           // PonFamily.text = GameStaticData.FamilyName[biological.FamilyId];


            PonInfoTog1.gameObject.SetActive(true);


            PonInfoTog2.gameObject.SetActive(true);
            

            PonInfoTog3.gameObject.SetActive(true);
        }
        #region  Faction
        private void ChangeFactionInfo()
        {
            ClearListItem();

            //Faction> factions = World.Active.GetExistingManager<FactionSystem>().GetFactions();
            //for (int i = 0; i < factions.Length; i++)
            //{
            //    RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_listItemPrefab, _listItemParent);
            //    UiListItem uiitem = item.GetComponent<UiListItem>();
            //    uiitem.Text.text = GameStaticData.LivingAreaName[factions[i].Id];
            //    uiitem.Id = factions[i].Id;
            //    uiitem.ClickCallback = delegate(UiListItem ui)
            //    {
            //        //LivingAreaWindowCD livingAreaUi = World.Active.GetExistingManager<LivingAreaSystem>().GetLivingAreaData(id);
            //        //_livingAreaNameTxt.text = GameStaticData.LivingAreaName[livingAreaUi.LivingAreaId];
            //        //_livingAreaDisTxt.text = GameStaticData.LivingAreaDescription[livingAreaUi.LivingAreaId];
            //    };
            //}
        }

        #endregion

        private void ChangeWuxueInfo()
        {
            ClearListItem();
        }
        private void ChangeLivingAreaInfo()
        {
            ClearListItem();

            //LivingAreaSystem system = World.Active.GetExistingManager<LivingAreaSystem>();
            //List<string> contents = system.GetLivingAreaNames();

            //for (int i = 0; i < contents.Count; i++)
            //{
            //    RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_listItemPrefab, _listItemParent);

            //    UiListItem uiitem = item.GetComponent<UiListItem>();
            //    uiitem.Text.text = contents[i];
            //    uiitem.ClickCallback = ShowLivingAreaInfo;
            //}
        }

        private void ShowLivingAreaInfo(UiListItem item)
        {


        }


    }
}

