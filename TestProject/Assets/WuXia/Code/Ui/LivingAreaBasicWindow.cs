﻿using System.Collections;
using System.Collections.Generic;
using WX;
using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
{
    public class LivingAreaBasicWindow : UIWindowBase
    {

        public Transform LivingAreaContent;
        [Header("详细")]
        [SerializeField]
        private Toggle StatusTog;             //状态
        [SerializeField]
        private RectTransform StatusView;
        [SerializeField]
        private Toggle BulidingTog;              //建筑
        [SerializeField]
        private RectTransform BulidingView;
        [SerializeField]
        private Toggle AnnualHistoryTog;              //年历
        [SerializeField]
        private RectTransform AnnualHistoryView;

        private LivingArea _curLivingArea;          //记录当前显示的生活区
        private LivingAreaWindowCD _livingAreaWindowCd;

        protected override void SetWindowId()
        {
            this.ID = WindowID.LivingAreaBasicWindow;
        }
        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.ForegroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }
        public override void InitWindowOnAwake()
        {
            LivingAreaContent.Find("Enter").GetComponent<Button>().onClick.AddListener(OnEnterLivingArea);
            StatusTog.onValueChanged.AddListener(StatusTogValueChanged);
            BulidingTog.onValueChanged.AddListener(BulidingTogValueChanged);
            AnnualHistoryTog.onValueChanged.AddListener(AnnualHistoryTogValueChanged);

            StatusTog.isOn = true;


        }

        /// <summary>
        /// 进入生活区方法 
        /// </summary>
        public void OnEnterLivingArea()
        {
            //if (_curLivingArea != null)
            //{
            //     StrategySceneControl.Instance.LivingAreaEnter(_curLivingArea);
            //}

            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);
            UICenterMasterManager.Instance.CloseWindow(this.ID);

        }

        private void StatusTogValueChanged(bool flag)
        {
            StatusView.gameObject.SetActive(flag);
        }
        private void BulidingTogValueChanged(bool flag)
        {
            BulidingView.gameObject.SetActive(flag);
        }
        private void AnnualHistoryTogValueChanged(bool flag)
        {
            AnnualHistoryView.gameObject.SetActive(flag);
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if(contextData==null) return;
            _livingAreaWindowCd = (LivingAreaWindowCD) contextData;
            
            LivingAreaContent.Find("Name").GetComponent<Text>().text= GameStaticData.LivingAreaName[_livingAreaWindowCd.LivingAreaId];
            LivingAreaContent.Find("Level").GetComponent<Text>().text=GameStaticData.LivingAreaLevel[_livingAreaWindowCd.LivingAreaLevel];
            LivingAreaContent.Find("Description").GetComponent<Text>().text=GameStaticData.LivingAreaDescription[_livingAreaWindowCd.LivingAreaId];

            //_curLivingArea = data.Node;
            // LivingAreaContent.Find("Name").GetComponent<Text>().text = GameStaticData.NameDic[content.OnlyEntity];
            //LivingAreaContent.Find("Description").GetComponent<Text>().text = _curLivingArea.Description;
            //LivingAreaContent.Find("Level").GetComponent<Text>().text = _curLivingArea.CurLevel.ToString();
            //LivingAreaContent.Find("Type").GetComponent<Text>().text = _curLivingArea.Type.ToString();
            //// LivingAreaContent.Find("Power").GetComponent<Text>().text = _curLivingArea.Value.PowerId.ToString();   
            //LivingAreaContent.Find("Renown").GetComponent<Text>().text = _curLivingArea.Renown.ToString();

            //LivingAreaContent.Find("HaveName").GetComponent<Text>().text = _curLivingArea.ToString();
            //  LivingAreaContent.Find("MoneyMax").GetComponent<Text>().text=node.LivingAreaMoneyMax.ToString();
            // LivingAreaContent.Find("MoneyValue").GetComponent<Text>().text = node.LivingAreaMoney.ToString();

        }
    }

}
