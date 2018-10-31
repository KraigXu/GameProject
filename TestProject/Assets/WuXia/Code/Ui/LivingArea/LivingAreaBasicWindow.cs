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

        private LivingAreaNode _curLivingArea;          //记录当前显示的生活区

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
            if (_curLivingArea != null)
            {
                StrategySceneControl.Instance.LivingAreaEnter(_curLivingArea);
            }


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
            _curLivingArea = data.Node;
            LivingAreaContent.Find("Name").GetComponent<Text>().text = _curLivingArea.Value.Name;
            LivingAreaContent.Find("Description").GetComponent<Text>().text = _curLivingArea.Value.Description;
            LivingAreaContent.Find("Level").GetComponent<Text>().text = _curLivingArea.Value.LivingAreaLevel.ToString();
            LivingAreaContent.Find("Type").GetComponent<Text>().text = _curLivingArea.Value.LivingAreaType.ToString();
           // LivingAreaContent.Find("Power").GetComponent<Text>().text = _curLivingArea.Value.PowerId.ToString();   //势力
            LivingAreaContent.Find("Renown").GetComponent<Text>().text = _curLivingArea.Renown.ToString();

            //LivingAreaContent.Find("HaveName").GetComponent<Text>().text = _curLivingArea.ToString();
          //  LivingAreaContent.Find("MoneyMax").GetComponent<Text>().text=node.LivingAreaMoneyMax.ToString();
           // LivingAreaContent.Find("MoneyValue").GetComponent<Text>().text = node.LivingAreaMoney.ToString();  //建筑  年表  进入

        }

        void Update()
        {

        }


        void OnDisable()
        {
            
        }


    }

}
