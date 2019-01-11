using System;
using System.Collections;
using System.Collections.Generic;
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
        private List<GameObject> _techniquesItems=new List<GameObject>();
        
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
        [SerializeField]
        private List<UiEquipmentItem> _equipmentItems;

        private BiologicalUiInData _uiData;
        private Biological _curBiological;

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
            _exitBtn.onClick.AddListener(delegate()
            {
                UICenterMasterManager.Instance.CloseWindow(this.ID);
            });

            _propertyTog.onValueChanged.AddListener(PropertyTogChange);
            _combatTog.onValueChanged.AddListener(CombatTogChange);
            _jiyiTog.onValueChanged.AddListener(JiyiTogChange);
            _tagTog.onValueChanged.AddListener(TagTogChange);

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
                    RectTransform rectGo = WXPoolManager.Pools[Define.PoolName].Spawn(_personnelPrefab, _personnelParent);
                    UiBiologicalAvatarItem item= rectGo.GetComponent<UiBiologicalAvatarItem>();
                    item.AvatarImage.sprite = GameStaticData.BiologicalAvatar[_uiData.Biologicals[i].BiologicalId];
                    item.Key = _uiData.Biologicals[i].BiologicalId;
                    item.ClickCallBack = BiologicalChange;
                }

                _name.text = GameStaticData.BiologicalNameDic[_curBiological.BiologicalId];
                _surname.text = GameStaticData.BiologicalSurnameDic[_curBiological.BiologicalId];

                PropertyTogChange(true);
            }
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
                _tizhitxt.text = _curBiological.Tizhi.ToString();
                _lidaotxt.text = _curBiological.AgeMax.ToString();

                _tizhitxt.text = _curBiological.Tizhi.ToString();
                _lidaotxt.text = _curBiological.Lidao.ToString();
                _jingshentxt.text = _curBiological.Jingshen.ToString();
                _lingdongtxt.text = _curBiological.Lingdong.ToString();
                _wuxingtxt.text = _curBiological.Wuxing.ToString();
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
                    RectTransform rectGo = WXPoolManager.Pools[Define.PoolName].Spawn(_techniquesPrefab, _jiyiContent);
                    rectGo.GetChild(0).GetComponent<Text>().text = GameStaticData.TechniquesName[content[i].Key];
                    rectGo.GetChild(1).GetComponent<Text>().text = content[i].Value.ToString();
                }
            }
            else
            {
                for (int i = 0; i < _techniquesItems.Count; i++)
                {
                    WXPoolManager.Pools[Define.PoolName].Despawn(_techniquesItems[i].transform);
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
           //EquipmentJsonData jsonData=  EquipmentSystem.GetEquipment(_curBiological.EquipmentId);
           
        }

        private void ChangeArticle()
        {

        }

        

    }
}