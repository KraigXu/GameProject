using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
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

        private void ButtonRest()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Rest();
        }

        private void ButtonTeam()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Team();
        }

        private void ButtonPerson()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Person();
        
        }

        private void ButtonLog()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Log();
          
        }

        private void ButtonIntelligence()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Intelligence();

        }

        private void ButtonMap()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Map();
            
        }

        private void ButtonOption()
        {
            World.Active.GetExistingManager<PlayerControlSystem>().Option();
        }


    }
}