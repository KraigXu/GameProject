using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WX;

namespace WX.Ui
{
    public class WXCharacterPanelWidow : UIWindowBase
    {

        [SerializeField] private Button _exitBtn;
        [SerializeField] private Text _name;
        [SerializeField] private Text _surname;

        [SerializeField] private Toggle _propertyTog;
        [SerializeField] private GameObject _propertyGo;
        [SerializeField] private Toggle _wuxueTog;
        [SerializeField] private GameObject _wuxueGo;
        [SerializeField] private Toggle _jiyiTog;
        [SerializeField] private GameObject _jiyiGo;
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

        protected override void SetWindowId()
        {
            this.ID = WindowID.WXCharacterPanelWindow;
        }

        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.ForegroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        public override void InitWindowOnAwake()
        {

            _exitBtn.onClick.AddListener(Exit);

            _propertyTog.onValueChanged.AddListener(PropertyTogChange);
            _wuxueTog.onValueChanged.AddListener(WuxueTogChange);
            _jiyiTog.onValueChanged.AddListener(JiyiTogChange);
            _tagTog.onValueChanged.AddListener(TagTogChange);

        }

        private void PropertyTogChange(bool flag)
        {
           _propertyGo.gameObject.SetActive(flag);
        }

        private void WuxueTogChange(bool flag)
        {
            _wuxueGo.SetActive(flag);
        }
        private void JiyiTogChange(bool flag)
        {
            _jiyiGo.SetActive(flag);
        }
        private void TagTogChange(bool flag)
        {
            _tagGo.SetActive(flag);
        }


        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            base.BeforeShowWindow(contextData);
            if (contextData != null)
            {
                BiologicalUiInData data = (BiologicalUiInData) contextData;
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

        private void Update()
        {

        }

        private void Exit()
        {
            UICenterMasterManager.Instance.CloseWindow(this.ID);
            
        }
    }
}