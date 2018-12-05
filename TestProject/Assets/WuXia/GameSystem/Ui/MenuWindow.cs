using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
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
            this._rest.onClick.AddListener(ButtonRest);
            this._team.onClick.AddListener(ButtonTeam);
            this._person.onClick.AddListener(ButtonPerson);
            this._log.onClick.AddListener(ButtonLog);
            this._intelligence.onClick.AddListener(ButtonIntelligence);
            this._map.onClick.AddListener(ButtonMap);
            this._option.onClick.AddListener(ButtonOption);

        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null) return;
            base.BeforeShowWindow(contextData);
            _data = (MenuEventData)contextData;
        }

        private void ButtonRest()
        {
            _data.Rest();
        }

        private void ButtonTeam()
        {
            _data.Team();
        }

        private void ButtonPerson()
        {
            _data.Person();
        }

        private void ButtonLog()
        {
            _data.Log();
        }

        private void ButtonIntelligence()
        {
            _data.Intelligence();
        }

        private void ButtonMap()
        {
            _data.Map();
        }

        private void ButtonOption()
        {
            _data.Option();
        }


    }
}