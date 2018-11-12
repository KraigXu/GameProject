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
        public Biological CurPlayer;


        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Input;
            public ComponentDataArray<Biological> Biological;
            public ComponentArray<AICharacterControl> AiControl;
        }

        [Inject]
        private PlayerData m_Players;

        private StrategyWindow _strategyWindow;

        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (StrategySceneInit.Settings == null)
                return;

            float dt = Time.deltaTime;
            for (int i = 0; i < m_Players.Length; ++i)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
                RaycastHit hit;    //声明一个碰撞的点
                Vector3 point=Vector3.zero;
                if (Physics.Raycast(ray, out hit))
                {
                    
                    if (Input.GetMouseButtonUp(0))
                    {
                        point = hit.point;
                       // m_Players.AiControl[i].SetTarget(hit.point);
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                    }

                    if (_curlastContact == hit.collider.transform) //表示为同一个物体
                    {
                    }
                    else //表示为不同物体
                    {

                    }
                }
                else
                {
                }
                switch ((LocationType)m_Players.Biological[i].LocationType)
                {
                    case LocationType.City:
                        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);
                        break;
                    case LocationType.Event:
                        break;
                    case LocationType.Field:
                        UpdatePlayerInput(i, dt);
                        if (point != Vector3.zero)
                        {
                            m_Players.AiControl[i].SetTarget(point);
                        }

                        // m_Players.NavMeshAgent[i].SetDestination(move);
                        //if (move.magnitude > 1f) move.Normalize();
                        //move = transform.InverseTransformDirection(move);
                        //CheckGroundStatus();
                        //move = Vector3.ProjectOnPlane(move, m_GroundNormal);
                        //m_TurnAmount = Mathf.Atan2(move.x, move.z);
                        //m_ForwardAmount = move.z;

                        //ApplyExtraTurnRotation();

                        //// control and velocity handling is different when grounded and airborne:
                        //if (m_IsGrounded)
                        //{
                        //    HandleGroundedMovement(crouch, jump);
                        //}
                        //else
                        //{
                        //    HandleAirborneMovement();
                        //}

                        //ScaleCapsuleForCrouching(crouch);
                        //PreventStandingInLowHeadroom();

                        //// send input and other state parameters to the animator
                        //UpdateAnimator(move);

                        ////_target

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
            BiologicalUiInData uidata=new BiologicalUiInData();
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

            uidata.BiologicalName = data.Surname+data.Name;
            uidata.Race = ComFun.GetRaceType((RaceType)data.RaceType);
            uidata.Sex = ComFun.GetSex((SexType) data.Sex);
            uidata.Prestige = ComFun.GetPrestige(data.Prestige);
            uidata.Influence = data.Influence.ToString();
            uidata.Disposition = data.Disposition.ToString();

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

        private void UpdatePlayerInput(int i, float dt)
        {
            PlayerInput pi;

            pi.Move.x = Input.GetAxis("Horizontal");
            pi.Move.y = 0.0f;
            pi.Move.z = Input.GetAxis("Vertical");
            pi.Shoot.x = 0;
            pi.Shoot.y = 0.0f;
            pi.Shoot.z = 0;
            pi.FireCooldown = 0;

            m_Players.Input[i] = pi;
        }

        public Transform _curlastContact;  //当前鼠标之前指向到的物体
        public Transform _clickTf;        //点击时效果坐标


        public bool _isManual = true; //是否手动 
        public Bounds _visibleRangeBounds;       //可视范围
        public float _targetDistance = 50f;
        public Vector3 _lastMousePosition;
        public float _originalNearPlane = 0.3f;
        public float _originalFarPlane = 1000f;
        public float _rotateSpeed = 3;
        public float _moveSpeed = 3;
        public float _zoomSpeed = 500f;
        public float _damping = 3f;
        public bool _isHold = false;
        public Vector3 _target=Vector3.zero;

        private void CameraCheck()
        {
            //if (EventSystem.current.IsPointerOverGameObject())
            //    return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
            RaycastHit hit;    //声明一个碰撞的点
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
              //  _clickTf.position = hit.point;

                if (Input.GetMouseButtonUp(0))
                {
                    _target = hit.point;
                    //agent.SetDestination()

                    //// CurMouseEffect.transform.position = point;
                    // CurPlayer.GetComponent<AICharacterControl>().SetTarget(CurMouseEffect.transform);
                    //NavMeshAgent agent = CurPlayer.GetComponent<NavMeshAgent>();
                    //LineRenderer moveLine = gameObject.GetComponent<LineRenderer>();
                    //if (moveLine == null)
                    //{
                    //    moveLine = gameObject.AddComponent<LineRenderer>();
                    //}

                    //设置路径的点，
                    //路径  导航。
                    //NavMeshPath path = new NavMeshPath();
                    //agent.CalculatePath(point, path);
                    ////线性渲染设置拐点的个数。数组类型的。
                    //moveLine.positionCount = path.corners.Length;
                    ////线性渲染的拐点位置，数组类型，
                    //agent.SetDestination(point);
                    //moveLine.SetPositions(path.corners);



                    //if (Mouse0ClickEvents.ContainsKey(hit.transform.tag))
                    //{
                    //    Mouse0ClickEvents[hit.transform.tag](hit.collider.transform, hit.point);
                    //}
                }

                if (Input.GetMouseButtonUp(1))
                {
                    //if (Mouse1ClickEvents.ContainsKey(hit.transform.tag))
                    //{
                    //    Mouse1ClickEvents[hit.transform.tag](hit.collider.transform, hit.point);
                    //}
                }

                if (_curlastContact == hit.collider.transform) //表示为同一个物体
                {
                    //if (MouseOverEvents.ContainsKey(hit.collider.tag))
                    //{
                    //    MouseOverEvents[hit.transform.tag](hit.collider.transform);
                    //}
                }
                else //表示为不同物体
                {
                    //if (MouseEnterEvents.ContainsKey(hit.transform.tag))
                    //{
                    //    MouseEnterEvents[hit.transform.tag](hit.collider.transform);
                    //}
                    //if (_curlastContact != null)
                    //{
                    //    if (MouseExitEvents.ContainsKey(_curlastContact.tag))
                    //    {
                    //        MouseExitEvents[_curlastContact.tag](_curlastContact);
                    //    }
                    //}

                }
                _curlastContact = hit.collider.transform;
            }
            else
            {
                if (_curlastContact != null)
                {
                    //if (MouseExitEvents.ContainsKey(_curlastContact.tag))
                    //{
                    //    MouseExitEvents[_curlastContact.tag](_curlastContact);
                    //}
                }
                _curlastContact = null;
            }

        }

        public void ShowPlayerUi()
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.WXCharacterPanelWindow);
        }

        private void CamerMove(Transform playerTf)
        {
            Vector3 _target = Vector3.zero;
            Camera _camera = Camera.main;
            Transform _cameraTf = _camera.GetComponent<Transform>();
            if (playerTf != null)
            {
                _target = playerTf.position;
            }

            if (Input.GetMouseButton(0))
            {
                _isManual = true;
            }
            else
            {
                _isManual = false;
            }


            if (_isManual)              //手动时以目标为中心
            {
                if (Input.GetMouseButton(1))
                {
                    if (_lastMousePosition != Vector3.zero)
                    {
                        Vector2 offset = Input.mousePosition - _lastMousePosition;
                        _cameraTf.RotateAround(_target, Vector3.up, offset.x * Time.deltaTime * _rotateSpeed);
                        _cameraTf.RotateAround(_target, _cameraTf.right, -offset.y * Time.deltaTime * _rotateSpeed);
                    }
                    _lastMousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButtonUp(1))
                {
                    _lastMousePosition = Vector3.zero;
                }

                if (Input.GetMouseButton(2))
                {
                    if (_lastMousePosition != Vector3.zero)
                    {
                        // float moveDistance = Time.deltaTime * -panSpeed * distance / 20f;
                        Vector3 offset = Input.mousePosition - _lastMousePosition;
                        _cameraTf.Translate(new Vector3(offset.x * Time.deltaTime * _moveSpeed, offset.y * Time.deltaTime * _moveSpeed, offset.z * Time.deltaTime * _moveSpeed));
                    }
                    _lastMousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButtonUp(2))
                {
                    _lastMousePosition = Vector3.zero;
                }

                if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                {
                    float moveValue = Input.GetAxis("Mouse ScrollWheel");
                    float moveDistance = moveValue * Time.deltaTime * _zoomSpeed;
                    _cameraTf.Translate(_cameraTf.forward * moveDistance);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 offset = new Vector3(0, 0, Time.deltaTime * _moveSpeed);
                    _cameraTf.Translate(offset, Space.World);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    Vector3 offset = new Vector3(0, 0, -Time.deltaTime * _moveSpeed);
                    _cameraTf.Translate(offset, Space.World);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 offset = new Vector3(-Time.deltaTime * _moveSpeed, 0, 0);
                    _cameraTf.Translate(offset, Space.World);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 offset = new Vector3(Time.deltaTime * _moveSpeed, 0, 0);
                    _cameraTf.Translate(offset, Space.World);
                }

            }
            else                    //跟随目标
            {
                Quaternion newrotation = Quaternion.Euler(60f, 30f, 0);
                Vector3 newposition = newrotation * new Vector3(0, 0, -50) + _target;
                _cameraTf.rotation = Quaternion.Lerp(_cameraTf.rotation, newrotation, Time.deltaTime * _damping);
                _cameraTf.position = Vector3.Lerp(_cameraTf.position, newposition, Time.deltaTime * _damping);
            }
            //ChangeCamerStatus
            _camera.nearClipPlane = _originalNearPlane;
            _camera.farClipPlane = _originalFarPlane;
        }



        public void Mouse0Click_LivingAreaMain(Transform target, Vector3 point)
        {
            Debug.Log(target.name + ">>Mouse0Click");
            LivingArea node = target.GetComponent<LivingArea>();
            //  _livingAreasSelect.position = node.LivingAreaRender.bounds.center;
            //  MessageBoxInstance.Instance.MessageBoxShow("");

            //判断逻辑

            //if (CurPlayer != null)
            //{
            //    Debuger.Log("Enter LivingAreas");
            //    CurPlayer.transform.position = node.transform.position;
            //    // M_Strategy.InstanceLivingArea(node);

            //    ShowWindowData showWindowData = new ShowWindowData();
            //    showWindowData.contextData = new WindowContextLivingAreaNodeData(node);
            //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

            //    if (CurPlayer.GroupId == -1)
            //    {
            //        //  M_Strategy.EnterLivingAreas(node, CurPlayer);
            //    }
            //    else
            //    {
            //        //  M_Strategy.EnterLivingAreas(node, M_Biological.GroupsDic[CurPlayer.GroupId].Partners);
            //    }
            //}
        }

        public void Mouse1Click_LivingAreaMain(Transform target, Vector3 point)
        {
            Debug.Log(target.name + ">>Mouse1Click");

            ShowWindowData showMenuData = new ShowWindowData();
            showMenuData.contextData = new WindowContextExtendedMenu(target.GetComponent<LivingArea>(), point);
            UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, showMenuData);
        }

        public void Mouse1Click_Terrain(Transform tf, Vector3 point)
        {
            Debuger.Log(">>>>>>>Terrain");
            // CurMouseEffect.transform.position = point;
            // CurPlayer.GetComponent<AICharacterControl>().SetTarget(CurMouseEffect.transform);
            // NavMeshAgent agent = CurPlayer.GetComponent<NavMeshAgent>();
            //LineRenderer moveLine = gameObject.GetComponent<LineRenderer>();
            //if (moveLine == null)
            //{
            //    moveLine = gameObject.AddComponent<LineRenderer>();
            //}

            ////设置路径的点，
            ////路径  导航。
            //NavMeshPath path = new NavMeshPath();
            //agent.CalculatePath(point, path);
            ////线性渲染设置拐点的个数。数组类型的。
            //moveLine.positionCount = path.corners.Length;
            ////线性渲染的拐点位置，数组类型，
            //agent.SetDestination(point);
            //moveLine.SetPositions(path.corners);
        }

     
        //显示Message
        public void MessageShow(string[] values)
        {
        }

    }


}

