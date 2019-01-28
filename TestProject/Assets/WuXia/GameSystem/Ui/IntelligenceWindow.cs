using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
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
        public Text LaNameTxt;
        public Text LaDescriptionTxt;
        public Text LaTypeTxt;
        public Text LaLevelTxt;

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

        /// <summary>
        /// 更新界面
        /// </summary>
        private void ChangePersonInfo()
        {
            ClearListItem();
            List<int> biologicalIds = World.Active.GetExistingManager<BiologicalSystem>().GetAllBiologicalName();
            for (int i = 0; i < biologicalIds.Count; i++)
            {
                RectTransform item = WXPoolManager.Pools[Define.PoolName].Spawn(_listItemPrefab, _listItemParent);

                UiListItem uiitem = item.GetComponent<UiListItem>();
                uiitem.Text.text = GameStaticData.BiologicalSurnameDic[biologicalIds[i]] + GameStaticData.BiologicalNameDic[biologicalIds[i]];
                uiitem.Id = biologicalIds[i];
                uiitem.ClickCallback = delegate (int id)
                {
                    Biological biological = World.Active.GetExistingManager<BiologicalSystem>().GetBiologicalInfo(id);
                    PonAvatr.sprite = GameStaticData.BiologicalAvatar[biological.AvatarId];
                    PonName.text = GameStaticData.BiologicalSurnameDic[biological.BiologicalId] + GameStaticData.BiologicalNameDic[biological.BiologicalId];
                    PonSex.text = GameStaticData.BiologicalSex[biological.SexId];
                    PonPrestige.text = World.Active.GetExistingManager<PrestigeSystem>().CheckValue(biological.PrestigeValue,ElementType.Biological);
                    PonFamily.text = GameStaticData.FamilyName[biological.FamilyId];
                };
                _listItems.Add(uiitem);
            }
        }

        #region  Faction
        private void ChangeFactionInfo()
        {
            ClearListItem();

            ComponentDataArray<Faction> factions = World.Active.GetExistingManager<FactionSystem>().GetFactions();
            for (int i = 0; i < factions.Length; i++)
            {
                RectTransform item = WXPoolManager.Pools[Define.PoolName].Spawn(_listItemPrefab, _listItemParent);
                UiListItem uiitem = item.GetComponent<UiListItem>();
                uiitem.Text.text = GameStaticData.LivingAreaName[factions[i].Id];
                uiitem.Id = factions[i].Id;
                uiitem.ClickCallback = delegate (int id)
                {
                    // LivingAreaWindowCD livingAreaUi = World.Active.GetExistingManager<LivingAreaSystem>().GetLivingAreaData(id);
                    //_livingAreaNameTxt.text = GameStaticData.LivingAreaName[livingAreaUi.LivingAreaId];
                    //_livingAreaDisTxt.text = GameStaticData.LivingAreaDescription[livingAreaUi.LivingAreaId];
                };
            }
        }

        #endregion

        private void ChangeWuxueInfo()
        {
            ClearListItem();
        }
        private void ChangeLivingAreaInfo()
        {
            ClearListItem();
            List<int> livngAreaIds = World.Active.GetExistingManager<LivingAreaSystem>().GetLivingAreaIds();
            for (int i = 0; i < livngAreaIds.Count; i++)
            {
                RectTransform item = WXPoolManager.Pools[Define.PoolName].Spawn(_listItemPrefab, _listItemParent);

                UiListItem uiitem = item.GetComponent<UiListItem>();
                uiitem.Text.text = GameStaticData.LivingAreaName[livngAreaIds[i]];
                uiitem.Id = livngAreaIds[i];
                uiitem.ClickCallback = delegate (int id)
                {
                    LivingArea livingArea = World.Active.GetExistingManager<LivingAreaSystem>().GetLivingAreaInfo(id);
                    LaNameTxt.text = GameStaticData.LivingAreaName[livingArea.Id];
                    LaDescriptionTxt.text = GameStaticData.LivingAreaDescription[livingArea.Id];
                    LaLevelTxt.text = GameStaticData.LivingAreaLevel[livingArea.CurLevel];
                  //  LaTypeTxt.text = GameStaticData.LivingAreaType[livingArea.TypeId];
                };
            }
        }
    }
}

