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

        [Header("PersonInfo")]
        [SerializeField]
        private RectTransform _personInfoContentParent;
        [SerializeField]
        private List<IntelligenceListItem> _personInfoItem;
        [SerializeField]
        private Image _personInfoImage;

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
            _personInfoTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _personInfoGo.SetActive(flag);
            });

            _factionTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _factionGo.SetActive(flag);
            });

            _familyTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _familyGo.SetActive(flag);
            });

            _wuxueTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _wuxueGo.SetActive(flag);
            });


        }

        /// <summary>
        /// 更新界面
        /// </summary>
        private void ChangePersonInfo()
        {
            

            World.Active.GetExistingManager<BiologicalSystem>().GetAllBiological();
        }



    }
}

