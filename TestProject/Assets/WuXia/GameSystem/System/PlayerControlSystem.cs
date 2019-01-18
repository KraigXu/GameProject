
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;
using GameSystem.Ui;
using Manager;

namespace GameSystem
{

    /// <summary>
    /// 玩家控制System
    /// </summary>
    public class PlayerControlSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<BiologicalStatus> Status;
        }


        [Inject]
        private LivingAreaSystem _livingAreaSystem;
        [Inject]
        private BiologicalSystem _biologicalSystem;

        [Inject]
        private Data _data;

        private EntityManager _entityManager;


        public enum PlayerStatus
        {
            None,  //无
            Normal, //模型存在且状态正常
            Stay,  //模型不存在且状态正常,
            Death,  //模型存在且状态死亡，
            Disappear, //模型不存在且状态死亡
        }

        private PlayerStatus _playerStatus;
        private GameObject _playerGo;


        private int _targetId;
        private Vector3 _targetPosition;
        private ElementType _targetType;
        private LocationType _targetLocationType;
        private bool _newIsInfo=false;


        void OnDestroy()
        {
            StrategyCameraManager.Instance.SingleStart -= MouseClick;
        }

        public PlayerControlSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        public void SetupInit()
        {
            StrategyCameraManager.Instance.SingleStart += MouseClick;
        }

        public void MouseClick(MouseInfo sender, MouseEventArgs args)
        {
            if (args.MouseButton == 0)
            {
                if (sender.go.tag == Define.TagLivingArea)
                {
                    GameObjectEntity gameObjectEntity = sender.go.GetComponent<GameObjectEntity>();
                    LivingArea livingArea=  _entityManager.GetComponentData<LivingArea>(gameObjectEntity.Entity);

                    var interaction = _entityManager.GetComponentData<InteractionElement>(gameObjectEntity.Entity);

                    _targetId = interaction.Id;
                    _targetPosition = interaction.Position;
                    _targetType = interaction.Type;
                    _targetLocationType = LocationType.City;
                    _newIsInfo = true;
                }else if (sender.go.tag == Define.TagTerrain)
                {
                    _targetId = -1;
                    _targetPosition = sender.Point;
                    _targetType = ElementType.Terrain;
                    _targetLocationType = LocationType.Field;
                    _newIsInfo = true;
                }
                else if (sender.go.tag == Define.TagBiological)
                {
                   //GameObjectEntity

                }


                //for (int i = 0; i < _data.Length; i++)
                //{
                //    if(_data.Status[i].BiologicalIdentity==0)
                //        continue;

                //    var status = _data.Status[i];
                //    status.TargetPosition = sender.Point;
                    
                //    if (sender.go.tag == Define.TagTerrain)
                //    {
                //        status.TargetLocationType = LocationType.Field;
                //        status.TargetType = ElementType.Terrain;
                //    }else if (sender.go.tag == Define.TagLivingArea)
                //    {
                //        status.TargetLocationType = LocationType.City;
                //        status.TargetType = ElementType.LivingArea;
                //    }else if (sender.go.tag == Define.TagBiological)
                //    {
                //        status.TargetLocationType = LocationType.Field;
                //        status.TargetType = ElementType.Biological;
                //    }
                //    _data.Status[i] = status;
                //}
            }
            else
            {

            }
        }

        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject() && _newIsInfo ==false)
                return;

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Status[i].BiologicalIdentity == 0)
                    continue;

                var status = _data.Status[i];
                status.TargetId = _targetId;
                status.TargetType = _targetType;
                status.TargetPosition = _targetPosition;
                status.TargetLocationType = _targetLocationType;
                _data.Status[i] = status;
            }
            _newIsInfo = false;

            //for (int i = 0; i < _data.Length; i++)
            //{
            //    var input = _data.Input[i];

            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;
            //    if (Physics.Raycast(ray, out hit))
            //    {
            //        Debug.DrawLine(ray.origin, hit.point, Color.red);
            //        input.MousePoint = hit.point;

            //    }
            //    else
            //    {
            //        input.MousePoint = Vector3.zero;
            //    }
            //    _data.Input[i] = input;
            //}

            //for (int i = 0; i < m_Players.Length; ++i)
            //{
            //    PlayerInput input = m_Players.Input[i];
            //    Biological biological = m_Players.Biological[i];
            //    BiologicalStatus newStatus = m_Players.Status[i];
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;
            //    if (Physics.Raycast(ray, out hit))
            //    {
            //        Debug.DrawLine(ray.origin, hit.point, Color.blue);
            //        input.MousePoint = hit.point;

            //        if (hit.collider.CompareTag(Define.TagBiological))
            //        {
            //            Entity entity = hit.collider.GetComponent<GameObjectEntity>().Entity;
            //            Biological tagBiological = EntityManager.GetComponentData<Biological>(entity);
            //            UICenterMasterManager.Instance.GetGameWindowScript<TipsWindow>(WindowID.TipsWindow).
            //                SetBiologicalTip(hit.point, tagBiological.BiologicalId);
            //        }

            //        if (Input.GetMouseButtonUp(0))
            //        {
            //            if (hit.collider.name.Contains(Define.TagTerrain))
            //            {
            //                m_Players.AiControl[i].SetTarget(hit.point);
            //                newStatus.TargetType = ElementType.Terrain;
            //                newStatus.TargetPosition = hit.point;
            //                return;
            //            }
            //            else if (hit.collider.CompareTag(Define.TagLivingArea))
            //            {
            //                LivingArea livingArea = _livingAreaSystem.GetLivingArea(hit.collider.transform);
            //                m_Players.AiControl[i].SetTarget(livingArea.Position);
            //                newStatus.TargetType = ElementType.LivingArea;
            //                newStatus.TargetId = livingArea.Id;

            //            }
            //            else if (hit.collider.CompareTag(Define.TagBiological))
            //            {
            //                Entity entity = hit.collider.GetComponent<GameObjectEntity>().Entity;
            //                Biological tagBiological = EntityManager.GetComponentData<Biological>(entity);

            //                newStatus.TargetType = ElementType.Biological;
            //                newStatus.TargetId = tagBiological.BiologicalId;

            //                m_Players.AiControl[i].SetTarget(hit.collider.transform);
            //            }
            //        }

            //        if (Input.GetMouseButtonUp(1))
            //        {
            //            if (hit.collider.CompareTag(Define.TagLivingArea))
            //            {
            //                LivingArea livingArea = _livingAreaSystem.GetLivingArea(hit.collider.transform);

            //                ShowWindowData windowData = new ShowWindowData();
            //                windowData.contextData = new ExtendedMenuWindowInData(LivingAreaOnClick, DistrictOnClick, hit.point, livingArea.Id);
            //                UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, windowData);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        input.MousePoint = Vector3.zero;
            //    }

            //    m_Players.Input[i] = input;
            //    newStatus.Position = m_Players.AiControl[i].transform.position;


            //    switch (m_Players.Status[i].LocationType)
            //    {
            //        case LocationType.None:
            //            break;
            //        case LocationType.Field:
            //            {
            //                //    newtarget.Target = m_Players.AiControl[i].transform.position;
            //            }
            //            break;
            //        case LocationType.LivingAreaEnter:
            //            {
            //                //行为
            //                //检查当前状态 显示UI信息 
            //                ShowWindowData windowData = new ShowWindowData();

            //                LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
            //                livingAreaWindowCd.LivingAreaId = m_Players.Status[i].TargetId;
            //                livingAreaWindowCd.OnOpen = LivingAreaOnOpen;
            //                livingAreaWindowCd.OnExit = LivingAreaOnExit;
            //                windowData.contextData = livingAreaWindowCd;

            //                SystemManager.Get<LivingAreaSystem>().ShowMainWindow(m_Players.Status[i].TargetId, windowData);
            //                // newtarget.Target = bounds.center;
            //                newStatus.LocationType = LocationType.LivingAreaIn;
            //            }
            //            break;
            //        case LocationType.LivingAreaIn:
            //            {
            //            }
            //            break;
            //        case LocationType.LivingAreaExit:
            //            {
            //            }
            //            break;
            //        case LocationType.SocialDialogEnter:
            //            {
            //                SocialDialogWindowData socialDialogWindowData = new SocialDialogWindowData();
            //                socialDialogWindowData.Aid = biological.BiologicalId;
            //                socialDialogWindowData.Bid = newStatus.TargetId;
            //                socialDialogWindowData.PangBaiId = 1;
            //                socialDialogWindowData.StartId = 1;
            //                socialDialogWindowData.StartlogId = new int[] { 1 };
            //                socialDialogWindowData.DialogEvent = SocialDialogEvent;
            //                socialDialogWindowData.Relation = RelationSystem.GetRelationValue(biological.BiologicalId, newStatus.TargetId);

            //                ShowWindowData windowData = new ShowWindowData();
            //                windowData.contextData = socialDialogWindowData;
            //                UICenterMasterManager.Instance.ShowWindow(WindowID.SocialDialogWindow, windowData);

            //                newStatus.LocationType = LocationType.SocialDialogIn;
            //            }
            //            break;
            //        case LocationType.SocialDialogIn:
            //            {
            //            }
            //            break;
            //        case LocationType.SocialDialogExit:
            //            {
            //                UICenterMasterManager.Instance.DestroyWindow(WindowID.SocialDialogWindow);

            //            }
            //            break;
            //    }
            //    // m_Players.Property[i] = newtarget;
            //    m_Players.Status[i] = newStatus;
            //}
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
            //for (int i = 0; i < m_Players.Length; i++)
            //{
            //    var value = m_Players.Property[i];
            //    value.Target = point;

            //    m_Players.Property[i] = value;
            //}
        }



        ///// <summary>
        ///// 获取当前Person
        ///// </summary>
        ///// <returns></returns>
        //public Biological GetCurrentPerson()
        //{
        //    return m_Players.Biological[0];
        //}

        //public BiologicalStatus GetCurrentStatus()
        //{
        //    return m_Players.Status[0];
        //}

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

