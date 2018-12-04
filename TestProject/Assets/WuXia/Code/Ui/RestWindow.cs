using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
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
        private Button _exitBtn;

        [Header("Project")]
        private List<GameObject> _headergo;
        

        private RestWindowInData _restWindowInData;

        protected override void SetWindowId()
        {
            this.ID = WindowID.RestWindow;
        }

        public override void InitWindowOnAwake()
        {
            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;

        }

        protected override void InitWindowCoreData()
        {
            _exitBtn.onClick.AddListener(CloseWindow);
            _waitTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _waitGo.SetActive(flag);
            });

            _wuxueTog.onValueChanged.AddListener(delegate(bool flag)
            {
                _wuxueGo.SetActive(flag);
                if (_restWindowInData.OnWuXueOpen != null)
                {
                    _restWindowInData.OnWuXueOpen();
                }
            });

        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if(contextData==null) return;
            base.BeforeShowWindow(contextData);
            _restWindowInData = (RestWindowInData)contextData;
        }


        protected override void CloseWindow()
        {
            if (_restWindowInData.OnExit != null)
            {
                _restWindowInData.OnExit();
            }
            base.CloseWindow();
        }



    }

}