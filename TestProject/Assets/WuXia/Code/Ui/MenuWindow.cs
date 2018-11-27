using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
{
    public class MenuWindow : UIWindowBase
    {
        [SerializeField]
        private Button _button1;
        [SerializeField]
        private Button _button2;
        [SerializeField]
        private Button _button3;
        [SerializeField]
        private Button _button4;
        [SerializeField]
        private Button _button5;
        [SerializeField]
        private Button _button6;

        private MenuEventData _data;

        protected override void SetWindowId()
        {
            this.ID = WindowID.MenuWindow;
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
            this._button1.onClick.AddListener(Button1Event);
            this._button2.onClick.AddListener(Button2Event);
            this._button3.onClick.AddListener(Button3Event);
            this._button4.onClick.AddListener(Button4Event);
            this._button5.onClick.AddListener(Button5Event);
            this._button6.onClick.AddListener(Button6Event);
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null) return;
            base.BeforeShowWindow(contextData);
            _data = (MenuEventData)contextData;
        }

        private void Button1Event()
        {
            _data.Rest();
        }

        private void Button2Event()
        {
            _data.Article();
        }
        private void Button3Event()
        {
            _data.Team();
        }
        private void Button4Event()
        {
            _data.Recording();
        }
        private void Button5Event()
        {
            _data.Log();
        }
        private void Button6Event()
        {
            _data.Relationship();
        }

    }
}