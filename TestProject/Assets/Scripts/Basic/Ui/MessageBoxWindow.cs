
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    public class MessageBoxWindow : UIWindowBase
    {

        public Button _cofirmBtn;
        public Button _cancelBtn;

        

        private MessageBoxWindowData _windowData;
        public override void InitWindowOnAwake()
        {
            ID = WindowID.MessageBoxWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        protected override void InitWindowData()
        {
            _cofirmBtn.onClick.AddListener(CofirmOnClick);
            _cancelBtn.onClick.AddListener(CancelOnClick);
          
        }

        

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData != null)
            {
                base.BeforeShowWindow(contextData);
            }

            _windowData = (MessageBoxWindowData) contextData;

            if (_windowData != null)
            {
                if (_windowData.Type == 0)
                {

                }
                else
                {

                }

            }
            else
            {

            }

            
            

        }

        private void CancelOnClick()
        {
            if (_windowData.CancelAction != null)
            {
                _windowData.CancelAction();
            }
        }

        private void CofirmOnClick()
        {
            if (_windowData.ConfirmAction!=null)
            {
                _windowData.ConfirmAction();
            }

        }

    }
}