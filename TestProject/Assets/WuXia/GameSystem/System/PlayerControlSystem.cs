
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
        [Inject]
        private LivingAreaSystem _livingAreaSystem;
        [Inject]
        private CameraSystem _cameraSystem;
        [Inject]
        private BiologicalSystem _biologicalSystem;

        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject() || StrategySceneInit.Settings == null || m_Players.Length == 0)
                return;

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
                            //检查当前状态 显示UI信息 

                            ShowWindowData windowData = new ShowWindowData();
                            LivingAreaWindowCD uidata = _livingAreaSystem.GetLivingAreaData(m_Players.Status[i].TargetId);

                            //_biologicalSystem.GetBiologicalOnLocation()
                            uidata.OnOpen = LivingAreaOnOpen;
                            uidata.OnExit = LivingAreaOnExit;
                            windowData.contextData = uidata;
                            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, windowData);

                            GameObject go = GameObject.Instantiate(GameStaticData.ModelPrefab[uidata.ModelId]);
                            Renderer[] renderers = go.transform.GetComponentsInChildren<Renderer>();

                            Bounds bounds = renderers[0].bounds;

                            for (int j = 1; j < renderers.Length; j++)
                            {
                                bounds.Encapsulate(renderers[j].bounds);
                            }
                            newtarget.Target = bounds.center;
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


        /// <summary>
        /// 获取当前Person
        /// </summary>
        /// <returns></returns>
        public Biological GetCurrentPerson()
        {
            return m_Players.Biological[0];

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

