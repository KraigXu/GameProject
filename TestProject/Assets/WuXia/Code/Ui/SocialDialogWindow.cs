using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
{
    public class SocialDialogWindow : UIWindowBase
    {
        private Image _leftImage;
        private Image _rightImage;

        private Text _leftText;
        private Text _rightText;
        private Text _zhegeText;

        protected override void SetWindowId()
        {
            this.ID = WindowID.SocialDialogWindow;
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
    }
}