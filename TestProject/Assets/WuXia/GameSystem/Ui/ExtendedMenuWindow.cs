using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    public class ExtendedMenuWindow : UIWindowBase
    {
        private GameObject _contentMenus;

        private ExtendedMenuWindowInData _info;
        [SerializeField]
        private BaseCorrespondenceByModelControl _contentUi;

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
            _contentMenus.transform.Find("PowerIntelligence").GetComponent<Button>().onClick.AddListener(PowerIntelligenceButton);
            _contentMenus.SetActive(true);
        }


        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null) return;

            _info = (ExtendedMenuWindowInData)contextData;

            _contentUi.Init(Camera.main, UICenterMasterManager.Instance._Camera, _info.Point);

            if (_info.DistrictEvent != null)
            {
                _contentMenus.transform.Find("CityIntelligence").GetComponent<Button>().onClick.AddListener(_info.DistrictEvent);
            }

            if (_info.LivingAreEvent != null)
            {
                _contentMenus.transform.Find("AreaIntelligence").GetComponent<Button>().onClick.AddListener(_info.LivingAreEvent);
            }

            _contentMenus.SetActive(true);
        }

        public void PowerIntelligenceButton()
        {
            DestroyWindow();
        }
    }
}

