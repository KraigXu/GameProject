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
        private List<UiListItem> _listItems=new List<UiListItem>();
        [SerializeField]
        private RectTransform _listItemParent;

        [SerializeField]
        private Button _exitBtn;

        [Header("PersonInfo")]
        [SerializeField]
        private Image _personInfoImage;
        [SerializeField]
        private Text _personName;
        [SerializeField]
        private Text _personProperty;
        [SerializeField]
        private Text _ponText;
        [SerializeField]
        private Text _pontText1;


        [Header("LivingAreaInfo")]
        [SerializeField]
        private Image _livingAreaImage;
        [SerializeField]
        private Text _livingAreaNameTxt;
        [SerializeField]
        private Text _livingAreaDisTxt;
        //[SerializeField]
        //private Text _livingArea



        protected override void SetWindowId()
        {
            this.ID = WindowID.IntelligenceWindow;
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
            _exitBtn.onClick.AddListener(delegate()
            {
                UICenterMasterManager.Instance.DestroyWindow(this.ID);
            });

            _personInfoTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _personInfoGo.SetActive(flag);
                if (flag)
                {
                    ChangePersonInfo();
                }
            });

            _factionTog.onValueChanged.AddListener(delegate (bool flag)
            {
                _factionGo.SetActive(flag);
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

            _livingAreaTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _livingAreaGo.SetActive(flag);
                if (flag)
                {
                    ChangeLivingAreaInfo();
                }
            });
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            base.BeforeShowWindow(contextData);
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
                RectTransform item = WXPoolManager.Pools[Define.PoolName].Spawn(_listItemPrefab,_listItemParent);

                UiListItem uiitem= item.GetComponent<UiListItem>();
                uiitem.Text.text=GameStaticData.BiologicalSurnameDic[biologicalIds[i]]+ GameStaticData.BiologicalNameDic[biologicalIds[i]];
                uiitem.Id = biologicalIds[i];
                uiitem.ClickCallback = ChangePersonCallback;
                

                _listItems.Add(uiitem);
            }

        }

        /// <summary>
        /// 项 被点击时  刷新Person界面，显示该人员信息
        /// </summary>
        /// <param name="id"></param>
        private void ChangePersonCallback(int id)
        {
            BiologicalUi biologicalUi= World.Active.GetExistingManager<BiologicalSystem>().GetBiologicalInfo(id);
            _personInfoImage.sprite = GameStaticData.BiologicalAvatar[biologicalUi.Id];
            _personName.text =GameStaticData.BiologicalSurnameDic[biologicalUi.Id]+GameStaticData.BuildingName[biologicalUi.Id];
            _personName.text = "1";
            
            _personProperty.text = "1";
        }

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
                uiitem.ClickCallback = ChangeLivingArea;
            }
        }

        private void ChangeLivingArea(int id)
        {
            LivingAreaWindowCD livingAreaUi = World.Active.GetExistingManager<LivingAreaSystem>().GetLivingAreaData(id);

            _livingAreaNameTxt.text = GameStaticData.LivingAreaName[livingAreaUi.LivingAreaId];
            _livingAreaDisTxt.text = GameStaticData.LivingAreaDescription[livingAreaUi.LivingAreaId];
            

        }
    }
}

