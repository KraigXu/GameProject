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
        [SerializeField]
        private Button _rest;
        [SerializeField]
        private Button _team;
        [SerializeField]
        private Button _person;
        [SerializeField]
        private Button _log;
        [SerializeField]
        private Button _intelligence;
        [SerializeField]
        private Button _map;
        [SerializeField]
        private Button _option;

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
            this._rest.onClick.AddListener(ButtonRest);
            this._team.onClick.AddListener(ButtonTeam);
            this._person.onClick.AddListener(ButtonPerson);
            this._log.onClick.AddListener(ButtonLog);
            this._intelligence.onClick.AddListener(ButtonIntelligence);
            this._map.onClick.AddListener(ButtonMap);
            this._option.onClick.AddListener(ButtonOption);
        }
        /// <summary>
        /// 修整按钮
        /// </summary>
        private void ButtonRest()
        {
            WorldTimeManager.Instance.Pause();
            UICenterMasterManager.Instance.ShowWindow(WindowID.RestWindow);
        }

        private void ButtonTeam()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.TeamWindow);
        }

        private void ButtonPerson()
        {
            var biological = SystemManager.Get<PlayerControlSystem>().GetCurrentPerson();
            
            BiologicalData data = SqlData.GetDataId<BiologicalData>(biological.BiologicalId);
            ShowWindowData showWindowData = new ShowWindowData();
            BiologicalUiInData uidata = new BiologicalUiInData();
            uidata.Age = biological.Age;
            uidata.AgeMax = biological.AgeMax;
            uidata.Tizhi = biological.Tizhi;
            uidata.Lidao = biological.Lidao;
            uidata.Jingshen = biological.Jingshen;
            uidata.Lingdong = biological.Lingdong;
            uidata.Wuxing = biological.Wuxing;
            uidata.Jing = biological.Jing;
            uidata.Qi = biological.Qi;
            uidata.Shen = biological.Shen;
            uidata.Sex = data.Sex;
            //uidata.Prestige = m_Players.Status[0].PrestigeValue;
            uidata.Id = biological.BiologicalId;

            //uidata.Influence = data.Influence;
            //uidata.Disposition = data.Disposition;
            // uidata.OnlyEntity = m_Players.Entity[0];
            showWindowData.contextData = uidata;

            UICenterMasterManager.Instance.ShowWindow(WindowID.WxCharacterPanelWindow, showWindowData);

        }

        private void ButtonLog()
        {
        }

        private void ButtonIntelligence()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.IntelligenceWindow);
        }
        private void ButtonMap()
        {
        }
        /// <summary>
        /// Option按钮
        /// </summary>
        private void ButtonOption()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.SettingMenuWindow);
        }


    }
}