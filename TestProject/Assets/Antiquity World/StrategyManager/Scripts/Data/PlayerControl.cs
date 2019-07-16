using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public bool IsEdit = false;

    void Start () {
		
	}

	void Update () {
	    if (Input.GetKeyUp(KeyCode.BackQuote))
	    {

	        IsEdit = !IsEdit;
	    }
    }

    void OnGUI()
    {
        if (IsEdit == true)
        {
            float windth = Screen.width * 0.6f;
            float height = Screen.height * 0.4f;

            GUI.Window(0, new Rect(Screen.width - windth, height, windth, height), Debuger.OnDebugWindow, "Debug");
        }
    }

}


///// <summary>
///// 玩家控制System
///// </summary>
//public class PlayerControlSystem : ComponentSystem
//{
//    struct Data
//    {
//        public readonly int Length;
//        public EntityArray Entity;
//        public GameObjectArray GameObjects;
//        public ComponentDataArray<PlayerInput> Input;
//        public ComponentDataArray<BehaviorData> Behavior;
//    }
//    struct InteractionData
//    {
//        public readonly int Length;
//        public EntityArray Entity;
//        public ComponentDataArray<InteractionElement> Interaction;
//        public ComponentDataArray<Position> Position;
//        public ComponentDataArray<Element> Element;
//        public GameObjectArray Array;
//    }

//    [Inject]
//    private Data _data;
//    [Inject]
//    private InteractionData _interactionData;
//    private EntityManager _entityManager;

//    public static Entity PlayerEntity
//    {
//        get { return SystemManager.Get<PlayerControlSystem>().Entitys[0]; }
//    }

//    public EntityArray Entitys
//    {
//        get { return _data.Entity; }
//    }

//    public PlayerControlSystem()
//    {
//        _entityManager = World.Active.GetOrCreateManager<EntityManager>();
//    }

//    protected override void OnUpdate()
//    {
//        for (int i = 0; i < _data.Length; i++)
//        {
//            var input = _data.Input[i];
//            var behavior = _data.Behavior[i];
//            var entity = _data.Entity[i];
//            if (input.MousePoint != Vector3.zero)
//            {
//                Touch(input);
//            }

//            if (input.ClickPoint != Vector3.zero)
//            {
//                _data.Behavior[i] = GetNewBehavior(_data.Entity[i], _data.GameObjects[i], input);

//            }

//            if (input.ViewMove != Vector2.zero)
//            {
//            }

//            if (behavior.TimeToLive <= 0 && behavior.TargetEntity != Entity.Null)
//            {
//                switch (behavior.TargetType)
//                {
//                    case ElementType.LivingArea:
//                        {
//                            LivingArea livingArea = _entityManager.GetComponentData<LivingArea>(behavior.TargetEntity);
//                            UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
//                            var model = _entityManager.GetComponentData<ModelComponent>(entity);
//                            model.Status = 1;
//                            _entityManager.SetComponentData(entity, model);
//                        }
//                        break;
//                    case ElementType.Biological:
//                        {

//                        }
//                        break;
//                    default:
//                        break;
//                }
//            }

//            //ChangeProperty

//        }

//        //for (int i = 0; i < _interactionData.Length; i++)
//        //{
//        //    var interaction = _interactionData.Interaction[i];
//        //    var position = _interactionData.Position[i];
//        //    var element = _interactionData.Element[i];
//        //    if (Vector3.Distance(position.Value, input.MousePoint) < interaction.Distance)
//        //    {
//        //        switch (element.Type)
//        //        {
//        //            case ElementType.None:
//        //                break;
//        //            case ElementType.Biological:
//        //                // Biological biological=_entityManager.GetComponentData<Biological>(_data.Entity[i]);
//        //                break;
//        //            case ElementType.District:
//        //                break;
//        //            case ElementType.LivingArea:
//        //                {
//        //                    behaviorData.Target = position.Value;
//        //                }
//        //                break;
//        //            case ElementType.Terrain:
//        //                break;
//        //            case ElementType.Team:
//        //                break;
//        //            default:
//        //                break;
//        //        }
//        //    }
//        //}

//        //if (EventSystem.current.IsPointerOverGameObject() && _newIsInfo == false)
//        //    return;

//        //for (int i = 0; i < _data.Length; i++)
//        //{
//        //    if (_data.Status[i].BiologicalIdentity == 0)
//        //        continue;

//        //    var status = _data.Status[i];
//        //    status.TargetId = _targetId;
//        //    status.TargetType = _targetType;
//        //    status.TargetPosition = _targetPosition;
//        //    status.TargetLocationType = _targetLocationType;
//        //    _data.Status[i] = status;
//        //}
//        //_newIsInfo = false;

//        //for (int i = 0; i < _data.Length; i++)
//        //{
//        //    var input = _data.Input[i];

//        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        //    RaycastHit hit;
//        //    if (Physics.Raycast(ray, out hit))
//        //    {
//        //        Debug.DrawLine(ray.origin, hit.point, Color.red);
//        //        input.MousePoint = hit.point;

//        //    }
//        //    else
//        //    {
//        //        input.MousePoint = Vector3.zero;
//        //    }
//        //    _data.Input[i] = input;
//        //}

//        //for (int i = 0; i < m_Players.Length; ++i)
//        //{
//        //    PlayerInput input = m_Players.Input[i];
//        //    Biological biological = m_Players.Biological[i];
//        //    BiologicalStatus newStatus = m_Players.Status[i];
//        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        //    RaycastHit hit;
//        //    if (Physics.Raycast(ray, out hit))
//        //    {
//        //        Debug.DrawLine(ray.origin, hit.point, Color.blue);
//        //        input.MousePoint = hit.point;

//        //        if (hit.collider.CompareTag(Define.TagBiological))
//        //        {
//        //            Entity entity = hit.collider.GetComponent<GameObjectEntity>().Entity;
//        //            Biological tagBiological = EntityManager.GetComponentData<Biological>(entity);
//        //            UICenterMasterManager.Instance.GetGameWindowScript<TipsWindow>(WindowID.TipsWindow).
//        //                SetBiologicalTip(hit.point, tagBiological.BiologicalId);
//        //        }

//        //        if (Input.GetMouseButtonUp(0))
//        //        {
//        //            if (hit.collider.name.Contains(Define.TagTerrain))
//        //            {
//        //                m_Players.AiControl[i].SetTarget(hit.point);
//        //                newStatus.TargetType = ElementType.Terrain;
//        //                newStatus.TargetPosition = hit.point;
//        //                return;
//        //            }
//        //            else if (hit.collider.CompareTag(Define.TagLivingArea))
//        //            {
//        //                LivingArea livingArea = _livingAreaSystem.GetLivingArea(hit.collider.transform);
//        //                m_Players.AiControl[i].SetTarget(livingArea.Position);
//        //                newStatus.TargetType = ElementType.LivingArea;
//        //                newStatus.TargetId = livingArea.Id;

//        //            }
//        //            else if (hit.collider.CompareTag(Define.TagBiological))
//        //            {
//        //                Entity entity = hit.collider.GetComponent<GameObjectEntity>().Entity;
//        //                Biological tagBiological = EntityManager.GetComponentData<Biological>(entity);

//        //                newStatus.TargetType = ElementType.Biological;
//        //                newStatus.TargetId = tagBiological.BiologicalId;

//        //                m_Players.AiControl[i].SetTarget(hit.collider.transform);
//        //            }
//        //        }

//        //        if (Input.GetMouseButtonUp(1))
//        //        {
//        //            if (hit.collider.CompareTag(Define.TagLivingArea))
//        //            {
//        //                LivingArea livingArea = _livingAreaSystem.GetLivingArea(hit.collider.transform);

//        //                ShowWindowData windowData = new ShowWindowData();
//        //                windowData.contextData = new ExtendedMenuWindowInData(LivingAreaOnClick, DistrictOnClick, hit.point, livingArea.Id);
//        //                UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, windowData);
//        //            }
//        //        }
//        //    }
//        //    else
//        //    {
//        //        input.MousePoint = Vector3.zero;
//        //    }

//        //    m_Players.Input[i] = input;
//        //    newStatus.Position = m_Players.AiControl[i].transform.position;


//        //    switch (m_Players.Status[i].LocationType)
//        //    {
//        //        case LocationType.None:
//        //            break;
//        //        case LocationType.Field:
//        //            {
//        //                //    newtarget.Target = m_Players.AiControl[i].transform.position;
//        //            }
//        //            break;
//        //        case LocationType.LivingAreaEnter:
//        //            {
//        //                //行为
//        //                //检查当前状态 显示UI信息 
//        //ShowWindowData windowData = new ShowWindowData();

//        //LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
//        //livingAreaWindowCd.LivingAreaId = m_Players.Status[i].TargetId;
//        //livingAreaWindowCd.OnOpen = LivingAreaOnOpen;
//        //livingAreaWindowCd.OnExit = LivingAreaOnExit;
//        //windowData.contextData = livingAreaWindowCd;

//        //SystemManager.Get<LivingAreaSystem>().ShowMainWindow(m_Players.Status[i].TargetId, windowData);
//        //// newtarget.Target = bounds.center;
//        //newStatus.LocationType = LocationType.LivingAreaIn;
//        //            }
//        //            break;
//        //        case LocationType.LivingAreaIn:
//        //            {
//        //            }
//        //            break;
//        //        case LocationType.LivingAreaExit:
//        //            {
//        //            }
//        //            break;
//        //        case LocationType.SocialDialogEnter:
//        //            {
//        //                SocialDialogWindowData socialDialogWindowData = new SocialDialogWindowData();
//        //                socialDialogWindowData.Aid = biological.BiologicalId;
//        //                socialDialogWindowData.Bid = newStatus.TargetId;
//        //                socialDialogWindowData.PangBaiId = 1;
//        //                socialDialogWindowData.StartId = 1;
//        //                socialDialogWindowData.StartlogId = new int[] { 1 };
//        //                socialDialogWindowData.DialogEvent = SocialDialogEvent;
//        //                socialDialogWindowData.Relation = RelationSystem.GetRelationValue(biological.BiologicalId, newStatus.TargetId);

//        //                ShowWindowData windowData = new ShowWindowData();
//        //                windowData.contextData = socialDialogWindowData;
//        //                UICenterMasterManager.Instance.ShowWindow(WindowID.SocialDialogWindow, windowData);

//        //                newStatus.LocationType = LocationType.SocialDialogIn;
//        //            }
//        //            break;
//        //        case LocationType.SocialDialogIn:
//        //            {
//        //            }
//        //            break;
//        //        case LocationType.SocialDialogExit:
//        //            {
//        //                UICenterMasterManager.Instance.DestroyWindow(WindowID.SocialDialogWindow);

//        //            }
//        //            break;
//        //    }
//        //    // m_Players.Property[i] = newtarget;
//        //    m_Players.Status[i] = newStatus;
//        //}
//    }

//    private BehaviorData GetNewBehavior(Entity entity, GameObject go, PlayerInput input)
//    {
//        var behavior = new BehaviorData();
//        if (input.MouseEntity != Entity.Null)
//        {

//            if (SystemManager.Contains<LivingArea>(input.MouseEntity) == true)
//            {
//                LivingArea livingArea = SystemManager.GetProperty<LivingArea>(input.MouseEntity);
//                behavior.Target = input.MousePoint;
//                behavior.TargetType = ElementType.LivingArea;
//                behavior.TargetId = livingArea.Id;
//                behavior.TimeToLive = 10;
//                behavior.TargetEntity = input.MouseEntity;
//            }
//        }
//        else
//        {
//            behavior.TargetId = -1;
//            behavior.Target = input.ClickPoint;
//            behavior.TargetType = ElementType.Terrain;
//        }

//        return behavior;
//        //go.GetComponent<AICharacterControl>().SetTarget(_data.Behavior[i].Target);

//        //bool flag = false;
//        //for (int i = 0; i < _interactionData.Length; i++)
//        //{
//        //    var interaction = _interactionData.Interaction[i];
//        //    var position = _interactionData.Position[i];
//        //    var element = _interactionData.Element[i];
//        //    if (Vector3.Distance(position.Value, input.ClickPoint) <= interaction.Distance)
//        //    {
//        //        flag = true;
//        //        switch (element.Type)
//        //        {
//        //            case ElementType.None:
//        //                break;
//        //            case ElementType.Biological:

//        //                Biological biological = SystemManager.GetProperty<Biological>(_interactionData.Entity[i]);

//        //                behavior.Target = input.ClickPoint+Vector3.up;
//        //                behavior.TargetId = biological.BiologicalId;
//        //                behavior.TargetType = ElementType.Biological;
//        //                break;
//        //            case ElementType.District:

//        //                break;
//        //            case ElementType.LivingArea:
//        //                {
//        //                    LivingArea livingArea = _entityManager.GetComponentData<LivingArea>(_interactionData.Entity[i]);
//        //                    Position livingareaPos = SystemManager.GetProperty<Position>(_interactionData.Entity[i]);
//        //                    behavior.Target = livingareaPos.Value;
//        //                    behavior.TargetType = ElementType.LivingArea;
//        //                    behavior.TargetId = livingArea.Id;
//        //                    behavior.TimeToLive = 10;
//        //                    behavior.TargetEntity = _interactionData.Entity[i];
//        //                }
//        //                break;
//        //            case ElementType.Terrain:
//        //                {
//        //                }
//        //                break;
//        //            case ElementType.Team:
//        //                { }
//        //                break;
//        //            default:
//        //                {
//        //                    behavior.TargetId = -1;
//        //                    behavior.Target = input.ClickPoint;
//        //                    behavior.TargetType = ElementType.Terrain;
//        //                }
//        //                break;
//        //        }
//        //    }
//        //}

//        //if (flag == false)
//        //{

//        //}
//        //return behavior;
//    }

//    /// <summary>
//    /// 初始化Player事件
//    /// </summary>
//    public void InitPlayerEvent(GameObject go)
//    {

//        ColliderTriggerEvent goevent = go.GetComponent<ColliderTriggerEvent>();
//        goevent.TriggerEnter = PlayerOnCollisionEnter;
//        goevent.TriggerExit = PlayerOnCollisionExit;
//    }


//    private void Touch(PlayerInput input)
//    {
//        for (int j = 0; j < _interactionData.Length; j++)
//        {
//            var interaction = _interactionData.Interaction[j];
//            var position = _interactionData.Position[j];
//            var element = _interactionData.Element[j];
//            if (Vector3.Distance(position.Value, input.MousePoint) < interaction.Distance)
//            {
//                switch (element.Type)
//                {
//                    case ElementType.None:
//                        break;
//                    case ElementType.Biological:
//                        // Biological biological=_entityManager.GetComponentData<Biological>(_data.Entity[i]);
//                        break;
//                    case ElementType.District:
//                        break;
//                    case ElementType.LivingArea:
//                        {
//                            //behaviorData.Target = position.Value;
//                        }
//                        break;
//                    case ElementType.Terrain:
//                        {
//                        }
//                        break;
//                    case ElementType.Team:
//                        { }
//                        break;
//                    default:
//                        {
//                        }
//                        break;
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// 当进入LivingArea时调用
//    /// </summary>
//    private void PlayerOnCollisionEnter(GameObject go, Collider other)
//    {
//        GameObjectEntity goEntity = go.GetComponent<GameObjectEntity>();
//        GameObjectEntity collisoneEntity = other.gameObject.GetComponent<GameObjectEntity>();
//        if (collisoneEntity)
//        {
//            BehaviorData target = SystemManager.GetProperty<BehaviorData>(goEntity.Entity);

//            if (target.TargetEntity == collisoneEntity.Entity)
//            {
//                if (SystemManager.Contains<LivingArea>(collisoneEntity.Entity))  //进入LivingArea判定
//                {
//                    int code = LivingAreaSystem.IsEnterLivingArea(goEntity.Entity, collisoneEntity.Entity);
//                    if (code == 0)
//                    {
//                        //播放进入动画

//                        //改变层级
//                        //go.SetLayerRecursively(LayerMask.NameToLayer("Hide"));
//                        //LivingAreaSystem.EnterLivingArea(goEntity.Entity, collisoneEntity.Entity);

//                        //ShowWindowData showWindowData=new ShowWindowData();
//                        //LivingAreaWindowCD livingAreaWindowCd=new LivingAreaWindowCD();
//                        //livingAreaWindowCd.LivingAreaEntity = collisoneEntity.Entity;
//                        //showWindowData.contextData = livingAreaWindowCd;
//                        //UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

//                    }
//                    else if (code == 1)
//                    {

//                    }
//                }

//            }
//        }
//    }



//    /// <summary>
//    /// 当退出时调用
//    /// </summary>
//    /// <param name="entity"></param>
//    /// <param name="id"></param>
//    private void PlayerOnCollisionExit(GameObject go, Collider other)
//    {
//        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
//        // BiologicalStatus status = entityManager.GetComponentData<BiologicalStatus>(entity);

//        // status.LocationType = LocationType.Field;
//    }

//    public void Target(Vector3 point)
//    {
//        //for (int i = 0; i < m_Players.Length; i++)
//        //{
//        //    var value = m_Players.Property[i];
//        //    value.Target = point;

//        //    m_Players.Property[i] = value;
//        //}
//    }



//    ///// <summary>
//    ///// 获取当前Person
//    ///// </summary>
//    ///// <returns></returns>
//    //public Biological GetCurrentPerson()
//    //{
//    //    return m_Players.Biological[0];
//    //}

//    //public BiologicalStatus GetCurrentStatus()
//    //{
//    //    return m_Players.Status[0];
//    //}

//    private void LivingAreaOnClick()
//    {

//    }

//    private void DistrictOnClick()
//    {

//    }


//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="result"></param>
//    /// <param name="a"></param>
//    /// <param name="b"></param>
//    /// <returns></returns>
//    private int[] SocialDialogEvent(int result, int a, int b)
//    {
//        return null;

//    }


//}
