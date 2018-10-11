using System.Collections;
using System.Collections.Generic;
using LivingArea;
using UnityEngine;
using UnityEngine.UI;

namespace TinyFrameWork
{
    public class LivingAreaBasicWindow : UIWindowBase
    {

        public Transform LivingAreaContent;
        public Text CityNameText;
        public Text CityLevelText;
        
        public Text CityPowerText;
        public Text CityHaveText;
        public Text CityDescriptionText;

        public Text ModenyText;
        public Text MoneyValueText;

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
            StatusTog.onValueChanged.AddListener(StatusTogValueChanged);
            BulidingTog.onValueChanged.AddListener(BulidingTogValueChanged);
            AnnualHistoryTog.onValueChanged.AddListener(AnnualHistoryTogValueChanged);

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
            WindowContextLivingAreaNodeData data = (WindowContextLivingAreaNodeData)contextData;
            if(data==null) return;

            LivingAreaNode node = data.Node;
            LivingAreaContent.Find("LivingAreaName").GetComponent<Text>().text = node.LivingAreaName;
            LivingAreaContent.Find("LivingAreaDescription").GetComponent<Text>().text = node.Description;
            LivingAreaContent.Find("LivingAreaLevel").GetComponent<Text>().text = node.BuildingLevel.ToString();
            LivingAreaContent.Find("Power").GetComponent<Text>().text = node.PowerId.ToString();
            LivingAreaContent.Find("Have").GetComponent<Text>().text = node.HaveId.ToString();
            LivingAreaContent.Find("MoneyMax").GetComponent<Text>().text=node.LivingAreaMoneyMax.ToString();
            LivingAreaContent.Find("MoneyValue").GetComponent<Text>().text = node.LivingAreaMoney.ToString();

        }

        void Update()
        {

        }


        void OnDisable()
        {
            
        }


    }

}
