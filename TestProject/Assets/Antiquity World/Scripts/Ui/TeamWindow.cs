using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{

    public class TeamWindow : UIWindowBase
    {

        [SerializeField]
        private GameObject _go;
        [SerializeField]
        private RectTransform _rect;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private GameObject _content;
        [SerializeField]
        private List<GameObject> _gos;

        [SerializeField]
        private List<GameObject> _go1;

        protected override void InitWindowData()
        {
            this.ID = WindowID.TeamWindow;

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

        public void Init()
        {

        }




    }
}