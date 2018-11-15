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
using Unity.Transforms;
using Unity.Mathematics;

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
            public ComponentDataArray<BiologicalStatus> Status;
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
                BiologicalStatus newStatus = m_Players.Status[i];
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
                                    newStatus.TargetType = 1;
                                    newStatus.TargetId = _livingAreaData.LivingArea[j].Id;
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
                            }
                        }
                    }
                }

                newStatus.Position = m_Players.AiControl[i].transform.position;
                if (m_Players.Status[i].StatusRealTime == (int)LocationType.City)
                {

                }
                else if (m_Players.Status[i].StatusRealTime == (int)LocationType.Event)
                {

                }
                else if (m_Players.Status[i].StatusRealTime == (int)LocationType.Field)
                {

                }
                else if (m_Players.Status[i].StatusRealTime == (int)LocationType.LivingAreaEnter)
                {
                    Debuger.Log("InLivingArea");
                    newStatus.StatusRealTime = (int)LocationType.LivingAreaIn;

                    GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(GameText.LivingAreaModelPath[m_Players.Status[i].TargetId]));
                    
                    //检查当前状态 显示UI信息 
                    LivingAreaWindowCD uidata = _livingAreaSystem.GetLivingAreaData(m_Players.Status[i].TargetId);
                    ShowWindowData windowData = new ShowWindowData();
                    windowData.contextData = uidata;
                    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, windowData);

                    // DataAccessObject.LivingAreaData data = SqlData.GetDataId<DataAccessObject.LivingAreaData>(m_Players.Status[i].TargetId);
                    // m_Players.Status[i] = newStatus;
                   
                    //m_Players.Status[i] = newStatus;
                    //GameObject model = GameObject.Instantiate(Resources.Load<GameObject>(data.ModelMain), go.transform);
                    //m_Players.Status[i] = newStatus;
                    //var entityManager = World.Active.GetOrCreateManager<EntityManager>();
                    //EntityArchetype buildingArchetype = entityManager.CreateArchetype(typeof(Building), typeof(Position));
                    //m_Players.Status[i] = newStatus;
                    //List<BuildingObject> buildings = JsonConvert.DeserializeObject<List<BuildingObject>>(data.BuildingInfoJson);
                    //m_Players.Status[i] = newStatus;
                    //for (int j = 0; j < buildings.Count; j++)
                    //{
                    //    Entity building = entityManager.CreateEntity(buildingArchetype);

                    //    entityManager.SetComponentData(building, new Building
                    //    {
                    //        DurableValue = buildings[j].DurableValue,
                    //        Level = buildings[j].BuildingLevel,
                    //        OwnId = buildings[j].OwnId,
                    //        Status = buildings[j].Status,
                    //        Type = buildings[j].Type,
                    //    });

                    //    entityManager.SetComponentData(building, new Position
                    //    {
                    //        Value = new float3(buildings[i].X, buildings[i].Y, buildings[i].Z)
                    //    });

                    //    GameText.BuildingNameDic.Add(building, buildings[i].Name);
                    //    GameText.BuildingDescriptionDic.Add(building, buildings[i].Description);

                    //    uidata.BuildingPoints.Add(new Vector3(buildings[i].X, buildings[i].Y, buildings[i].Z));
                    //    uidata.Buildings.Add(building);
                    //    uidata.BuildingAlats.Add(buildings[i].ImageId);
                    //}
                    ////GameText

                    //Entity entity = go.GetComponent<GameObjectEntity>().Entity;
                    //m_Players.Status[i] = newStatus;
                    //entityManager.AddComponent(entity, ComponentType.Create<LivingAreaMain>());

                    //ShowWindowData windowData = new ShowWindowData();
                    //windowData.contextData = uidata;
                    //UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, windowData);
                }
                m_Players.Status[i] = newStatus;
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
            //switch ((TragetType)code)
            //{
            //    case TragetType.City:
            //        {
            //            //Loading LivingArea
            //            _livingAreaSystem.OpenLivingArea(targetId);
            //        }
            //        break;
            //    case TragetType.Field:
            //        _worldTimeSystem.Pause();
            //        break;
            //    case TragetType.Idie:
            //        break;
            //}

        }


        private void LivingAreaOnClick()
        {

        }

        private void DistrictOnClick()
        {

        }

    }


}

