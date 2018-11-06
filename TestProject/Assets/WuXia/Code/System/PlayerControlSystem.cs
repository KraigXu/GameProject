using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using WX;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace  WX
{
    public class PlayerControlSystem : ComponentSystem
    {
        public Biological CurPlayer;


        struct PlayerData
        {

#pragma warning disable 649
            public PlayerInput Input;
#pragma warning restore 649
        }
        struct PlayerGroup
        {
            public Player player;
            public PlayerInput playerInput;
            public Biological biological;
        }

        public static void SetupComponentData(EntityManager entityManager)
        {
        }

        /// <summary>
        /// 显示用户数据
        /// </summary>
        /// <param name="entityManager"></param>
        public static void SetupPlayerView(EntityManager entityManager)
        {
            DemoSetting demoSetting= GameObject.Find("Setting").GetComponent<DemoSetting>();
            if (demoSetting.StartType == 0)
            {
                GameObject playerGo=GameObject.Find("Biological").transform.Find(demoSetting.PlayerId.ToString()).gameObject;
                Player player = playerGo.AddComponent<Player>();
                PlayerInput playerInput= playerGo.AddComponent<PlayerInput>();
                UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow).GetComponent<StrategyWindow>();
            }
            else
            {

            }
        }

        protected override void OnCreateManager()
        {
            Debuger.Log(">>>");
            //m_MainGroup = GetComponentGroup(typeof(District));
        }


        protected override void OnStartRunning()
        {
            base.OnStartRunning();


          MouseEnterEvents.Add("Player", MouseEnter_PlayerMain);
          MouseExitEvents.Add("Player", MouseExit_PlayerMain);
          MouseOverEvents.Add("Player", MouseOver_PlayerMain);
          Mouse0ClickEvents.Add("Player", Mouse0Click_PlayerMain);
          Mouse1ClickEvents.Add("Player", Mouse1Click_PlayerMain);

          MouseEnterEvents.Add("LivingArea", MouseEnter_LivingAreaMain);
          MouseExitEvents.Add("LivingArea", MouseExit_LivingAreaMain);
          MouseOverEvents.Add("LivingArea", MouseOver_LivingAreaMain);
          Mouse0ClickEvents.Add("LivingArea", Mouse0Click_LivingAreaMain);
          Mouse1ClickEvents.Add("LivingArea", Mouse1Click_LivingAreaMain);

          MouseEnterEvents.Add("Terrain", MouseEnter_Terrain);
          MouseExitEvents.Add("Terrain", MouseExit_Terrain);
          MouseOverEvents.Add("Terrain", MouseOver_Terrain);
          Mouse0ClickEvents.Add("Terrain", Mouse0Click_Terrain);
          Mouse1ClickEvents.Add("Terrain", Mouse1Click_Terrain);

          MouseEnterEvents.Add("Biological", MouseEnter_Biological);
          MouseExitEvents.Add("Biological", MouseExit_Biological);
          MouseOverEvents.Add("Biological", MouseOver_Biological);
          Mouse0ClickEvents.Add("Biological", Mouse0Click_Biological);
          Mouse1ClickEvents.Add("Biological", Mouse1Click_Biological);

        }


        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;
            foreach (var entity in GetEntities<PlayerGroup>())
            {
                CameraCheck();
                CamerMove(entity.biological.transform);
            }


        }

        public delegate void OnMousePointing(Transform tf);
        public delegate void OnMousePointingPoint(Transform tf, Vector3 point);
        /// <summary>
        /// 当标识的鼠标进入时
        /// </summary>
        public Dictionary<string, OnMousePointing> MouseEnterEvents = new Dictionary<string, OnMousePointing>();
        public Dictionary<string, OnMousePointing> MouseExitEvents = new Dictionary<string, OnMousePointing>();
        public Dictionary<string, OnMousePointing> MouseOverEvents = new Dictionary<string, OnMousePointing>();
        public Dictionary<string, OnMousePointingPoint> Mouse0ClickEvents = new Dictionary<string, OnMousePointingPoint>();
        public Dictionary<string, OnMousePointingPoint> Mouse1ClickEvents = new Dictionary<string, OnMousePointingPoint>();

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

        private void CameraCheck()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
            RaycastHit hit;    //声明一个碰撞的点
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
                _clickTf.position = hit.point;
                if (Input.GetMouseButtonUp(0))
                {
                    if (Mouse0ClickEvents.ContainsKey(hit.transform.tag))
                    {
                        Mouse0ClickEvents[hit.transform.tag](hit.collider.transform, hit.point);
                    }
                }

                if (Input.GetMouseButtonUp(1))
                {
                    if (Mouse1ClickEvents.ContainsKey(hit.transform.tag))
                    {
                        Mouse1ClickEvents[hit.transform.tag](hit.collider.transform, hit.point);
                    }
                }

                if (_curlastContact == hit.collider.transform) //表示为同一个物体
                {
                    if (MouseOverEvents.ContainsKey(hit.collider.tag))
                    {
                        MouseOverEvents[hit.transform.tag](hit.collider.transform);
                    }
                }
                else //表示为不同物体
                {
                    if (MouseEnterEvents.ContainsKey(hit.transform.tag))
                    {
                        MouseEnterEvents[hit.transform.tag](hit.collider.transform);
                    }
                    if (_curlastContact != null)
                    {
                        if (MouseExitEvents.ContainsKey(_curlastContact.tag))
                        {
                            MouseExitEvents[_curlastContact.tag](_curlastContact);
                        }
                    }

                }


                _curlastContact = hit.collider.transform;
            }
            else
            {
                if (_curlastContact != null)
                {
                    if (MouseExitEvents.ContainsKey(_curlastContact.tag))
                    {
                        MouseExitEvents[_curlastContact.tag](_curlastContact);
                    }
                }
                _curlastContact = null;
            }

        }

        private void CamerMove(Transform playerTf)
        {
            Vector3 _target = Vector3.zero;
            Camera _camera=Camera.main;
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


        public void MouseEnter_PlayerMain(Transform tf)
        {
            Debug.Log(tf.name + ">>MouseEnter");
        }
        public void MouseExit_PlayerMain(Transform tf)
        {
            Debug.Log(tf.name + ">>MouseExit");
        }
        public void MouseOver_PlayerMain(Transform tf)
        {
            Debug.Log(tf.name + ">> MouseOver");
        }
        public void Mouse0Click_PlayerMain(Transform tf, Vector3 point)
        {
            Debug.Log(tf.name + ">>Mouse0Click");
        }
        public void Mouse1Click_PlayerMain(Transform tf, Vector3 point)
        {
            Debug.Log(tf.name + ">>Mouse1Click");
        }

        public void MouseEnter_LivingAreaMain(Transform tf)
        {
            Debug.Log(tf.name + ">>MouseEnter");
        }
        public void MouseExit_LivingAreaMain(Transform tf)
        {
            Debug.Log(tf.name + ">>MouseExit");
        }
        public void MouseOver_LivingAreaMain(Transform target)
        {
            Debug.Log(target.name + ">> MouseOver");
        }
        public void Mouse0Click_LivingAreaMain(Transform target, Vector3 point)
        {
            Debug.Log(target.name + ">>Mouse0Click");
            LivingArea node = target.GetComponent<LivingArea>();
          //  _livingAreasSelect.position = node.LivingAreaRender.bounds.center;
            //  MessageBoxInstance.Instance.MessageBoxShow("");

            //判断逻辑

            if (CurPlayer != null)
            {
                Debuger.Log("Enter LivingAreas");
                CurPlayer.transform.position = node.transform.position;
                // M_Strategy.InstanceLivingArea(node);

                ShowWindowData showWindowData = new ShowWindowData();
                showWindowData.contextData = new WindowContextLivingAreaNodeData(node);
                UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

                if (CurPlayer.GroupId == -1)
                {
                    //  M_Strategy.EnterLivingAreas(node, CurPlayer);
                }
                else
                {
                    //  M_Strategy.EnterLivingAreas(node, M_Biological.GroupsDic[CurPlayer.GroupId].Partners);
                }
            }
        }
        public void Mouse1Click_LivingAreaMain(Transform target, Vector3 point)
        {
            Debug.Log(target.name + ">>Mouse1Click");

            ShowWindowData showMenuData = new ShowWindowData();
            showMenuData.contextData = new WindowContextExtendedMenu(target.GetComponent<LivingArea>(), point);
            UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, showMenuData);
        }

        public void MouseEnter_Terrain(Transform tf)
        {
        }

        public void MouseExit_Terrain(Transform tf)
        {

        }

        public void MouseOver_Terrain(Transform tf)
        {

        }

        public void Mouse0Click_Terrain(Transform tf, Vector3 point)
        {
         //   CurMouseEffect.transform.position = point;
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

        public void MouseEnter_Biological(Transform tf)
        {

        }

        public void MouseExit_Biological(Transform tf)
        {

        }

        public void MouseOver_Biological(Transform tf)
        {

        }

        public void Mouse0Click_Biological(Transform tf, Vector3 point)
        {

        }

        public void Mouse1Click_Biological(Transform tf, Vector3 point)
        {

        }



        //显示Message
        public void MessageShow(string[] values)
        {
        }

        public void MessageShow(string value)
        {

        }
    }


}

