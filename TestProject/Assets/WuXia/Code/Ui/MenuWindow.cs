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

        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            base.BeforeShowWindow(contextData);
            if (contextData ==null)return;
            _data = (MenuEventData) contextData;

            for (int i = 0; i < _data.Names.Count; i++)
            {

            }

        }
    }
}