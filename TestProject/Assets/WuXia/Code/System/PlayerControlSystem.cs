using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using WX;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;

namespace WX
{
    public enum PlayerType
    {
        WatchingWar,
        ParticipatingWar,
        Dying
    }



    public class PlayerControlSystem : ComponentSystem
    {

        struct LivingAreaData
        {
            public readonly int Length;
            public ComponentDataArray<LivingArea> LivingArea;
            public ComponentArray<BoxCollider> Collider;
        }
        [Inject]
        private LivingAreaData _livingAreaData;

        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Input;
            public ComponentDataArray<Biological> Biological;
            public ComponentArray<AICharacterControl> AiControl;
            public ComponentDataArray<Prestige> Prestige;
            public EntityArray Entity;
        }

        [Inject]
        private PlayerData m_Players;
        [Inject]
        private WorldTimeSystem _worldTimeSystem;
        [Inject]
        private LivingAreaSystem _livingAreaSystem;
        [Inject]
        private CameraSystem _cameraSystem;

        private StrategyWindow _strategyWindow;
        

        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject() || StrategySceneInit.Settings == null || m_Players.Length == 0)
                return;

            float dt = Time.deltaTime;
            for (int i = 0; i < m_Players.Length; ++i)
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
                RaycastHit hit;    //声明一个碰撞的点
                bool flag = false;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.blue);

                    if (Input.GetMouseButtonUp(0))
                    {
                        if (hit.collider.tag == "Terrain")
                        {
                            m_Players.AiControl[i].SetTarget(hit.point, ContactTarget, (int)TragetType.Field, -1);
                            return;
                        }
                        else if (hit.collider.tag == "LivingArea")
                        {
                            for (int j = 0; j < _livingAreaData.Length; j++)
                            {
                                if (_livingAreaData.Collider[j].bounds.Contains(hit.point))
                                {
                                    m_Players.AiControl[i].SetTarget(_livingAreaData.Collider[j].bounds.center, ContactTarget, (int)TragetType.City, _livingAreaData.LivingArea[j].Id);
                                    return;
                                }
                            }
                        }
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        for (int j = 0; j < _livingAreaData.Length; j++)
                        {
                            if (_livingAreaData.Collider[j].bounds.Contains(hit.point))
                            {
                                ShowWindowData windowData = new ShowWindowData();
                                windowData.contextData = new ExtendedMenuWindowInData(LivingAreaOnClick, DistrictOnClick, hit.point, _livingAreaData.LivingArea[j].Id);
                                flag = true;
                                return;
                            }
                        }
                    }


                }
                var biological = m_Players.Biological[i];
                var input = m_Players.Input[i];


                switch ((LocationType)biological.LocationType)
                {
                    case LocationType.City:
                        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);
                        break;
                    case LocationType.Event:
                        break;
                    case LocationType.Field:
                        
                        break;
                }
            }

           

            if (_strategyWindow == null)
            {
                ShowWindowData data = new ShowWindowData();
                data.contextData = new StrategyWindowInData(PlayerInfoUi, ShowGFUi, TechnologyUi, LogEvent, MapEvent);
                _strategyWindow = UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow, data).GetComponent<StrategyWindow>();
            }
        }



        //-----------------------UI
        private void PlayerInfoUi()
        {

            if (m_Players.Length == 0)
            {
                return;
            }
            var biological = m_Players.Biological[0];
            BiologicalData data = SqlData.GetDataId<BiologicalData>(m_Players.Biological[0].BiologicalId);

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
            uidata.Prestige = m_Players.Prestige[0].Level;
            uidata.Influence = data.Influence;
            uidata.Disposition = data.Disposition;
            uidata.OnlyEntity = m_Players.Entity[0];            

            showWindowData.contextData = uidata;
            UICenterMasterManager.Instance.ShowWindow(WindowID.WXCharacterPanelWindow, showWindowData);

        }

        private void ShowGFUi()
        {

        }

        private void TechnologyUi()
        {

        }

        private void LogEvent()
        {

        }

        private void MapEvent()
        {

        }

        private void ContactTarget(int code, int targetId)
        {
            switch ((TragetType)code)
            {
                case TragetType.City:
                    {
                        //Loading LivingArea
                        _livingAreaSystem.OpenLivingArea(targetId);
                    }
                    break;
                case TragetType.Field:
                    _worldTimeSystem.Pause();
                    break;
                case TragetType.Idie:
                    break;
            }

        }


        private void LivingAreaOnClick()
        {

        }

        private void DistrictOnClick()
        {

        }

    }


}

