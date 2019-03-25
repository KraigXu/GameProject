using System;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld.StrategyManager;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;
using Unity.Entities;

namespace GameSystem.Ui
{
    public class WXCharacterPanelWidow : UIWindowBase
    {
        [SerializeField] private Transform _personnelParent;
        [SerializeField] private RectTransform _personnelPrefab;

        [Header("Introduction")]
        [SerializeField] private Toggle _introductionTog;
        [SerializeField] private GameObject _introductionGo;
        [SerializeField] private Button _exitBtn;
        [SerializeField] private Text _name;
        [SerializeField] private Text _surname;

        [SerializeField] private Toggle _propertyTog;
        [SerializeField] private GameObject _propertyGo;

        [SerializeField] private Toggle _combatTog;
        [SerializeField] private GameObject _combatGo;

        [SerializeField] private Toggle _jiyiTog;
        [SerializeField] private Transform _jiyiGo;
        [SerializeField] private RectTransform _techniquesPrefab;
        [SerializeField] private Transform _jiyiContent;
        private List<GameObject> _techniquesItems = new List<GameObject>();

        [SerializeField] private Toggle _tagTog;
        [SerializeField] private GameObject _tagGo;

        [Header("Property")]
        [SerializeField] private Text _tizhitxt;
        [SerializeField] private Text _lidaotxt;
        [SerializeField] private Text _jingshentxt;
        [SerializeField] private Text _lingdongtxt;
        [SerializeField] private Text _wuxingtxt;
        [SerializeField] private Text _neigongtxt;
        [SerializeField] private Text _waigongtxt;
        [SerializeField] private Text _jingtxt;
        [SerializeField] private Text _qitxt;
        [SerializeField] private Text _shentxt;

        [Header("Equipment")]
        [SerializeField] private List<GameObject> _equipmentItems = new List<GameObject>();

        [Header("Article")]
        private ArticleManager _articleManager = new ArticleManager();

        // [SerializeField] private RectTransform _equipment
        private BiologicalUiInData _uiData;
        private Biological _curBiological;

        private int _curShowId;
        private Transform _showTransform;

        protected override void InitWindowData()
        {
            this.ID = WindowID.WxCharacterPanelWindow;

            windowData.windowType = UIWindowType.ForegroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        public override void InitWindowOnAwake()
        {

            _exitBtn.onClick.AddListener(delegate ()
            {
                UICenterMasterManager.Instance.CloseWindow(this.ID);
            });

            _propertyTog.onValueChanged.AddListener(PropertyTogChange);
            _combatTog.onValueChanged.AddListener(CombatTogChange);
            _jiyiTog.onValueChanged.AddListener(JiyiTogChange);
            _tagTog.onValueChanged.AddListener(TagTogChange);

            for (int i = 0; i < _equipmentItems.Count; i++)
            {
                UIEventTriggerListener.Get(_equipmentItems[i]).onEnter = ShowEquipmentInfo;
                UIEventTriggerListener.Get(_equipmentItems[i]).onExit = CloseEquipmentInfo;
            }
        }

        private void ShowEquipmentInfo(GameObject go)
        {
            Entity biologicalEntity = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(_curShowId);
            _showTransform = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategySceneInit.Settings.ArticleInfoPerfab.transform, transform);

            UiEquipmentItem equipment = _showTransform.gameObject.GetComponent<UiEquipmentItem>();

            string[] nameValue = go.name.Split('_');  //type_Id

            switch ((ArticleType)int.Parse(nameValue[0]))
            {
                case ArticleType.Coat:
                    {
                        if (SystemManager.Contains<EquipmentCoat>(biologicalEntity))
                        {
                            UiEquipmentStyle style = new UiEquipmentStyle();
                            EquipmentCoat equipmentCoat = SystemManager.GetProperty<EquipmentCoat>(biologicalEntity);

                            style.Title = "equipmentCoat";
                            style.Level = (int)EquipLevel.General;
                            style.conents = new Dictionary<string, List<string>>();
                            style.Values = new Dictionary<string, string>();
                            style.Values.Add("钝器防御:", equipmentCoat.BluntDefense.ToString());
                            style.Values.Add("利器防御:", equipmentCoat.SharpDefense.ToString());
                            style.Values.Add("操作性:", equipmentCoat.Operational.ToString());

                            style.Values.Add("重量:", equipmentCoat.Weight.ToString());
                            style.Values.Add("价格:", equipmentCoat.Price.ToString());
                            style.Values.Add("耐久:", equipmentCoat.Durable.ToString());
                            style.BackgroundId = equipmentCoat.SpriteId;


                            //equipmentCoat
                        }
                        else
                        {
                        }

                    }
                    break;
                case ArticleType.Gloves:
                    break;
                case ArticleType.Pants:
                    break;
            }

            //UICenterMasterManager.Instance.ShowWindow(id:)
        }

        private void CloseEquipmentInfo(GameObject go)
        {
            WXPoolManager.Pools[Define.GeneratedPool].Despawn(_showTransform);
            //WXPoolManager.Pools[Define.PoolName].Despawn(_items)
            //WXPoolManager.Pools[Define.PoolName].Despawn(_items[i].node);
        }



        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            base.BeforeShowWindow(contextData);
            if (contextData != null)
            {
                _uiData = (BiologicalUiInData)contextData;
                _curBiological = _uiData.CurPlayer;

                for (int i = 0; i < _uiData.Biologicals.Count; i++)
                {
                    RectTransform rectGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_personnelPrefab, _personnelParent);
                    UiBiologicalAvatarItem item = rectGo.GetComponent<UiBiologicalAvatarItem>();
                    item.AvatarImage.sprite = GameStaticData.BiologicalAvatar[_uiData.Biologicals[i].BiologicalId];
                    item.Key = _uiData.Biologicals[i].BiologicalId;
                    item.ClickCallBack = BiologicalChange;
                }

                _name.text = GameStaticData.BiologicalNameDic[_curBiological.BiologicalId];
                _surname.text = GameStaticData.BiologicalSurnameDic[_curBiological.BiologicalId];
                PropertyTogChange(true);
            }


        }

        void BeforeShow()
        {
            PlayerControlSystem system = SystemManager.Get<PlayerControlSystem>();
        }




        void Update()
        {

        }

        /// <summary>
        /// 切换人物信息
        /// </summary>
        /// <param name="key"></param>
        private void BiologicalChange(int key)
        {
            for (int i = 0; i < _uiData.Biologicals.Count; i++)
            {
                if (_uiData.Biologicals[i].BiologicalId == key)
                {
                    _curBiological = _uiData.Biologicals[i];
                }
            }

            _name.text = GameStaticData.BiologicalNameDic[_curBiological.BiologicalId];
            _surname.text = GameStaticData.BiologicalSurnameDic[_curBiological.BiologicalId];

            PropertyTogChange(_propertyTog.isOn);
            CombatTogChange(_combatTog.isOn);
            JiyiTogChange(_jiyiTog.isOn);
            TagTogChange(_tagTog.isOn);
            ChangeEquipment();
            ChangeArticle();
        }

        private void PropertyTogChange(bool flag)
        {
            _propertyGo.gameObject.SetActive(flag);
            if (flag == true)
            {
                //_tizhitxt.text = _curBiological.Tizhi.ToString();
                //_lidaotxt.text = _curBiological.AgeMax.ToString();
                //_tizhitxt.text = _curBiological.Tizhi.ToString();
                //_lidaotxt.text = _curBiological.Lidao.ToString();
                //_jingshentxt.text = _curBiological.Jingshen.ToString();
                //_lingdongtxt.text = _curBiological.Lingdong.ToString();
                //_wuxingtxt.text = _curBiological.Wuxing.ToString();
                _jingtxt.text = _curBiological.Jing.ToString();
                _qitxt.text = _curBiological.Qi.ToString();
                _shentxt.text = _curBiological.Shen.ToString();
            }
            else
            {
            }
        }

        private void CombatTogChange(bool flag)
        {
            _combatGo.gameObject.SetActive(flag);
            if (flag == true)
            {

            }
            else
            {

            }
        }
        /// <summary>
        /// Techniques面板打开 更新和清除
        /// </summary>
        /// <param name="flag"></param>
        private void JiyiTogChange(bool flag)
        {
            _jiyiGo.gameObject.SetActive(flag);

            if (flag == true)
            {
                List<KeyValuePair<int, int>> content = TechniquesSystem.GetTechnique(_curBiological.TechniquesId).Content;

                for (int i = 0; i < content.Count; i++)
                {
                    RectTransform rectGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_techniquesPrefab, _jiyiContent);
                    rectGo.GetChild(0).GetComponent<Text>().text = GameStaticData.TechniquesName[content[i].Key];
                    rectGo.GetChild(1).GetComponent<Text>().text = content[i].Value.ToString();
                }
            }
            else
            {
                for (int i = 0; i < _techniquesItems.Count; i++)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(_techniquesItems[i].transform);
                }
                _techniquesItems.Clear();
            }
        }
        private void TagTogChange(bool flag)
        {
            _tagGo.gameObject.SetActive(flag);
            if (flag == true)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        private void ChangeEquipment()
        {
            Entity biologicalEntity = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(_curShowId);

            if (SystemManager.Contains<EquipmentCoat>(biologicalEntity))
            {
                EquipmentCoat equipmentCoat = SystemManager.GetProperty<EquipmentCoat>(biologicalEntity);
            }
            else
            {
            }





        }

        private void ChangeArticle()
        {
            Entity biologicalEntity = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(_curShowId);

            GameObject playgo = StrategySceneInit.EcsGameObjectsDic[biologicalEntity];
            if (playgo == null)
            {
                Debug.LogError(">>>>>>>>>>ChangeArticle Error");
            }

            // BiologicalBehaviourComponent component= playgo.GetComponent<BiologicalBehaviourComponent>();
            //_articleManager.Init()


        }
    }
}