using DataAccessObject;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{

    /// <summary>
    /// 功能菜单界面
    /// </summary>
    public class MenuWindow : UIWindowBase
    {

        protected override void InitWindowData()
        {
            this.ID = WindowID.MenuWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
        }


        public override void InitWindowOnAwake()
        {

        }

        /// <summary>
        /// 修整按钮
        /// </summary>
        public void ButtonRest()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.RestWindow);
        }

        public void ButtonTeam()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.TeamWindow);
        }

        /// <summary>
        /// 获取人物信息
        /// </summary>
        public void ButtonPerson()
        {

            UICenterMasterManager.Instance.ShowWindow(WindowID.WXCharacterPanelWindow);

        }

        public void ButtonLog()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.LogWindow);
        }

        /// <summary>
        /// Intelligence
        /// </summary>
        public void ButtonIntelligence()
        {
            
            UICenterMasterManager.Instance.ShowWindow(WindowID.IntelligenceWindow);
           // UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow);
        }

        /// <summary>
        /// Map
        /// </summary>
        public void ButtonMap()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);
        }
        /// <summary>
        /// Option按钮
        /// </summary>
        public void ButtonOption()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.SettingMenuWindow);
        }


    }
}