
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;
using GameSystem.Ui;

namespace GameSystem
{

    /// <summary>
    /// 玩家控制System
    /// </summary>
    public class PlayerControlSystem : ComponentSystem
    {
        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Input;
            public ComponentDataArray<Biological> Biological;
            public ComponentArray<AICharacterControl> AiControl;
            public ComponentDataArray<BiologicalStatus> Status;
            public ComponentDataArray<CameraProperty> Property;
            public ComponentDataArray<InteractionElement> Interaction;
            public EntityArray Entity;
        }

        [Inject]
        private PlayerData m_Players;


        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Input;
        }


        [Inject]
        private LivingAreaSystem _livingAreaSystem;
        [Inject]
        private CameraSystem _cameraSystem;
        [Inject]
        private BiologicalSystem _biologicalSystem;

        [Inject]
        private PlayerData _player;
        [Inject]
        private Data _data;

        private EntityManager _entityManager;

        public PlayerControlSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }



        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject() || StrategySceneInit.Settings == null || m_Players.Length == 0)
                return;

            for (int i = 0; i < _data.Length; i++)
            {
                var input = _data.Input[i];

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    input.MousePoint = hit.point;

                }
                else
                {
                    input.MousePoint = Vector3.zero;
                }
                _data.Input[i] = input;
            }

            for (int i = 0; i < m_Players.Length; ++i)
            {
                PlayerInput input = m_Players.Input[i];
                Biological biological = m_Players.Biological[i];
                BiologicalStatus newStatus = m_Players.Status[i];
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.blue);
                    input.MousePoint = hit.point;

                    if (hit.collider.CompareTag(Define.TagBiological))
                    {
                        Entity entity = hit.collider.GetComponent<GameObjectEntity>().Entity;
                        Biological tagBiological = EntityManager.GetComponentData<Biological>(entity);
                        UICenterMasterManager.Instance.GetGameWindowScript<TipsWindow>(WindowID.TipsWindow).
                            SetBiologicalTip(hit.point, tagBiological.BiologicalId);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        if (hit.collider.name.Contains(Define.TagTerrain))
                        {
                            m_Players.AiControl[i].SetTarget(hit.point);
                            newStatus.TargetType = ElementType.Terrain;
                            newStatus.TargetPosition = hit.point;
                            return;
                        }
                        else if (hit.collider.CompareTag(Define.TagLivingArea))
                        {
                            LivingArea livingArea = _livingAreaSystem.GetLivingArea(hit.collider.transform);
                            m_Players.AiControl[i].SetTarget(livingArea.Position);
                            newStatus.TargetType = ElementType.LivingArea;
                            newStatus.TargetId = livingArea.Id;

                        }
                        else if (hit.collider.CompareTag(Define.TagBiological))
                        {
                            Entity entity = hit.collider.GetComponent<GameObjectEntity>().Entity;
                            Biological tagBiological = EntityManager.GetComponentData<Biological>(entity);

                            newStatus.TargetType = ElementType.Biological;
                            newStatus.TargetId = tagBiological.BiologicalId;

                            m_Players.AiControl[i].SetTarget(hit.collider.transform);
                        }
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        if (hit.collider.CompareTag(Define.TagLivingArea))
                        {
                            LivingArea livingArea = _livingAreaSystem.GetLivingArea(hit.collider.transform);

                            ShowWindowData windowData = new ShowWindowData();
                            windowData.contextData = new ExtendedMenuWindowInData(LivingAreaOnClick, DistrictOnClick, hit.point, livingArea.Id);
                            UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, windowData);
                        }
                    }
                }
                else
                {
                    input.MousePoint = Vector3.zero;
                }

                m_Players.Input[i] = input;
                newStatus.Position = m_Players.AiControl[i].transform.position;
                CameraProperty newtarget = m_Players.Property[i];

                switch (m_Players.Status[i].LocationType)
                {
                    case LocationType.None:
                        break;
                    case LocationType.Field:
                        {
                            newtarget.Target = m_Players.AiControl[i].transform.position;
                        }
                        break;
                    case LocationType.LivingAreaEnter:
                        {
                            //行为
                            //检查当前状态 显示UI信息 
                            ShowWindowData windowData = new ShowWindowData();

                            LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
                            livingAreaWindowCd.LivingAreaId = m_Players.Status[i].TargetId;
                            livingAreaWindowCd.OnOpen = LivingAreaOnOpen;
                            livingAreaWindowCd.OnExit = LivingAreaOnExit;
                            windowData.contextData = livingAreaWindowCd;

                            SystemManager.Get<LivingAreaSystem>().ShowMainWindow(m_Players.Status[i].TargetId, windowData);
                            // newtarget.Target = bounds.center;
                            newStatus.LocationType = LocationType.LivingAreaIn;
                        }
                        break;
                    case LocationType.LivingAreaIn:
                        {
                        }
                        break;
                    case LocationType.LivingAreaExit:
                        {
                        }
                        break;
                    case LocationType.SocialDialogEnter:
                        {
                            SocialDialogWindowData socialDialogWindowData = new SocialDialogWindowData();
                            socialDialogWindowData.Aid = biological.BiologicalId;
                            socialDialogWindowData.Bid = newStatus.TargetId;
                            socialDialogWindowData.PangBaiId = 1;
                            socialDialogWindowData.StartId = 1;
                            socialDialogWindowData.StartlogId = new int[] { 1 };
                            socialDialogWindowData.DialogEvent = SocialDialogEvent;
                            socialDialogWindowData.Relation = RelationSystem.GetRelationValue(biological.BiologicalId, newStatus.TargetId);

                            ShowWindowData windowData = new ShowWindowData();
                            windowData.contextData = socialDialogWindowData;
                            UICenterMasterManager.Instance.ShowWindow(WindowID.SocialDialogWindow, windowData);

                            newStatus.LocationType = LocationType.SocialDialogIn;
                        }
                        break;
                    case LocationType.SocialDialogIn:
                        {
                        }
                        break;
                    case LocationType.SocialDialogExit:
                        {
                            UICenterMasterManager.Instance.DestroyWindow(WindowID.SocialDialogWindow);

                        }
                        break;
                }
                m_Players.Property[i] = newtarget;
                m_Players.Status[i] = newStatus;
            }
        }

        /// <summary>
        /// 当进入LivingArea时调用
        /// </summary>
        private void LivingAreaOnOpen(Entity entity, int id)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            BiologicalStatus status = entityManager.GetComponentData<BiologicalStatus>(entity);
            status.LocationType = LocationType.LivingAreaIn;
        }

        /// <summary>
        /// 当退出时调用
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        private void LivingAreaOnExit(Entity entity, int id)
        {
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            BiologicalStatus status = entityManager.GetComponentData<BiologicalStatus>(entity);

            status.LocationType = LocationType.Field;
        }

        public void Target(Vector3 point)
        {
            for (int i = 0; i < m_Players.Length; i++)
            {
                var value = m_Players.Property[i];
                value.Target = point;

                m_Players.Property[i] = value;
            }
        }



        /// <summary>
        /// 获取当前Person
        /// </summary>
        /// <returns></returns>
        public Biological GetCurrentPerson()
        {
            return m_Players.Biological[0];
        }

        public BiologicalStatus GetCurrentStatus()
        {
            return m_Players.Status[0];
        }

        private void LivingAreaOnClick()
        {

        }

        private void DistrictOnClick()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int[] SocialDialogEvent(int result, int a, int b)
        {
            return null;

        }


    }


}

