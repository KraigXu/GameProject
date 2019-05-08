using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

namespace GameSystem.Ui
{
    /// <summary>
    /// 用户基础信息
    /// </summary>
    public class PlayerInfoWindow : UIWindowBase
    {

        public Image Image;
        public int Id;
        public Entity Entity;
        public EntityArray EntityArray;



        protected override void InitWindowData()
        {
            this.ID = WindowID.PlayerInfoWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;

        }


        public override void InitWindowOnAwake()
        {

        }


        void Update()
        {
        }

    }
}