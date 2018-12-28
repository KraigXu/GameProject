using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    /// <summary>
    /// 休整界面
    /// </summary>
    public class RestWindow : UIWindowBase
    {
        
        [SerializeField]
        private Toggle _waitTog;
        [SerializeField]
        private GameObject _waitGo;
        
        [SerializeField]
        private Toggle _wuxueTog;
        [SerializeField]
        private  GameObject _wuxueGo;
        [SerializeField]
        private Toggle _productionTog;
        [SerializeField]
        private GameObject _productionGo;
        [SerializeField]
        private Toggle _recuperateTog;
        [SerializeField]
        private GameObject _recuperateGo;

        [SerializeField]
        private Button _exitBtn;

        [Header("Project")]
        private List<GameObject> _headergo;
        
        protected override void InitWindowData()
        {
            this.ID = WindowID.RestWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
        }

        public override void InitWindowOnAwake()
        {
            _exitBtn.onClick.AddListener(CloseBtnMain);
            _waitTog.onValueChanged.AddListener(ChangeWaitView);
            _wuxueTog.onValueChanged.AddListener(ChangeWuxueView);
            _productionTog.onValueChanged.AddListener(ChangeProductionView);
            _recuperateTog.onValueChanged.AddListener(ChangeRecuperateView);
        }

        protected override void CloseWindow()
        {
            WorldTimeManager.Instance.Play();
        }


        private void CloseBtnMain()
        {
            UICenterMasterManager.Instance.CloseWindow(this.ID);
        }


        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="flag"></param>
        private void ChangeWaitView(bool flag)
        {
            _waitGo.SetActive(flag);
            if (flag)
            {
                
            }
            else
            {

            }
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="flag"></param>
        private void ChangeWuxueView(bool flag)
        {


        }

        private void ChangeProductionView(bool flag)
        {
        }

        private void ChangeRecuperateView(bool flag)
        {

        }

    }

}