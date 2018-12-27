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

        [SerializeField] private Button _exitBtn;
        [SerializeField] private Text _name;
        [SerializeField] private Text _surname;

        public Toggle PersonTog;
        public GameObject PersonGo;
        public Toggle WuxueTog;
        public GameObject WuxueGo;


        [SerializeField] private Toggle _propertyTog;
        [SerializeField] private GameObject _propertyGo;
        [SerializeField] private Toggle _wuxueTog;
        [SerializeField] private GameObject _wuxueGo;


        [SerializeField] private Toggle _jiyiTog;
        [SerializeField] private GameObject _jiyiGo;
        [SerializeField] private GameObject _techniquesPrefab;
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

        [SerializeField]
        private Biological _biological;
    
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
            _wuxueTog.onValueChanged.AddListener(WuxueTogChange);
            _jiyiTog.onValueChanged.AddListener(JiyiTogChange);
            _tagTog.onValueChanged.AddListener(TagTogChange);

        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            base.BeforeShowWindow(contextData);
            if (contextData != null)
            {
                BiologicalUiInData data = (BiologicalUiInData)contextData;
                _name.text = GameStaticData.BiologicalNameDic[data.Id];
                _surname.text = GameStaticData.BiologicalSurnameDic[data.Id];

                _tizhitxt.text = data.Tizhi.ToString();
                _lidaotxt.text = data.AgeMax.ToString();

                _tizhitxt.text = data.Tizhi.ToString();
                _lidaotxt.text = data.Lidao.ToString();
                _jingshentxt.text = data.Jingshen.ToString();
                _lingdongtxt.text = data.Lingdong.ToString();
                _wuxingtxt.text = data.Wuxing.ToString();
                _jingtxt.text = data.Jing.ToString();
                _qitxt.text = data.Qi.ToString();
                _shentxt.text = data.Shen.ToString();
            }
        }

        private void PropertyTogChange(bool flag)
        {
           _propertyGo.gameObject.SetActive(flag);
        }

        private void WuxueTogChange(bool flag)
        {
            _wuxueGo.SetActive(flag);
        }
        /// <summary>
        /// Techniques面板打开 更新和清除
        /// </summary>
        /// <param name="flag"></param>
        private void JiyiTogChange(bool flag)
        {
            _jiyiGo.SetActive(flag);

            if (flag == true)
            {
                List<Techniques> techniqueses =SystemManager.Get<TechniquesSystem>().GetIdTechniques(_biological.BiologicalId);

                for (int i = 0; i < techniqueses.Count; i++)
                {
                    GameObject go = UGUITools.AddChild(_jiyiGo, _techniquesPrefab);
                    
                    _techniquesItems.Add(go);
                }
            }
            else
            {
                for (int i = 0; i < _techniquesItems.Count; i++)
                {
                    Destroy(_techniquesItems[i]);
                }
                _techniquesItems.Clear();
            }
        }
        private void TagTogChange(bool flag)
        {
            _tagGo.SetActive(flag);
        }

    }
}