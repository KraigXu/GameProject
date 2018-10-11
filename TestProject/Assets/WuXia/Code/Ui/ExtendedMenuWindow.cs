using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyFrameWork
{
    public class ExtendedMenuWindow : UIWindowBase
    {
        private GameObject _contentMenus;

        private WindowContextExtendedMenu _info;
        protected override void SetWindowId()
        {
            this.ID = WindowID.ExtendedMenuWindow;
        }

        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }

        public override void InitWindowOnAwake()
        {
            _contentMenus = transform.Find("Content").gameObject;
            _contentMenus.transform.Find("CityIntelligence").GetComponent<Button>().onClick.AddListener(CityIntelligenceButton);
            _contentMenus.transform.Find("PowerIntelligence").GetComponent<Button>().onClick.AddListener(PowerIntelligenceButton);
            _contentMenus.transform.Find("AreaIntelligence").GetComponent<Button>().onClick.AddListener(AreaIntelligenceButton);
            _contentMenus.SetActive(false);
        }


        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
             _info = (WindowContextExtendedMenu)contextData;
            if (_info == null) return;

            ExtendedMenuContentItem contentItem = _contentMenus.GetComponent<ExtendedMenuContentItem>();
            contentItem.Wordpos = _info.Point;
            
            _contentMenus.SetActive(true);
        }

        public void CityIntelligenceButton()
        {
            ShowWindowData data=new ShowWindowData();
            data.contextData = new WindowContextLivingAreaNodeData(_info.LivingAreaNodeCom);

            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaBasicWindow, data);

            DestroyWindow();
        }

        public void PowerIntelligenceButton()
        {
            DestroyWindow();
        }

        public void AreaIntelligenceButton()
        {

           
            
            DestroyWindow();
        }


    }
}

