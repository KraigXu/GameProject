﻿using System;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class MouseInfo
    {
        public GameObject go;
        public Vector3 Point;
    }

    public class MouseEventArgs : EventArgs
    {
        public int MouseButton = 0;
        public Vector2 TouchPoint = Vector2.zero;
    }

    /// <summary>
    /// 全局相机控制
    /// </summary>
    public class StrategyCameraManager : MonoBehaviour
    {
        public static StrategyCameraManager _instance;

        public static StrategyCameraManager Instance
        {
            get { return _instance; }
        }

        public delegate void TouchInputEventHandler<T, U>(T sender, U e);

        public event TouchInputEventHandler<MouseInfo, MouseEventArgs> SingleStart;

        public Vector3 Target;
        public Vector3 RoationOffset = new Vector3(50, 0, 0);
        public float Distance = 10;
        public Vector3 Offset = new Vector3(0, 1, -15);
        public float Damping = 3;
        public float ZoomSpeed;

        public Transform CameraTf;
        public bool IsFollow;
        public float Speed = 3.5f;

        public LayerMask Layer;
        public Vector3 MousePoint;
        public GameObject MouseTarget;
        public RtsCamera RtsCamera;

        public GameObject TargetOj;

        void Awake()
        {
            _instance = this;
            CameraTf = transform;
        }

        void Start()
        {
            StrategySceneInit.InitializeWithScene();

            SetTarget(TargetOj.transform.position,true);

        }


        void OnGUI()
        {

            //if (GUI.Button(new Rect(0, 0, 160, 60), "战斗测试"))
            //{
            //    World.DisposeAllWorlds();
            //    WXSceneManager.Load("FightingScene");
            //    //SceneManager.LoadScene("FightingScene");
            //}
        }


        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetKey(KeyCode.W))
            {
                IsFollow = false;
                CameraTf.position += Vector3.forward * Time.deltaTime * Speed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                IsFollow = false;
                CameraTf.position += Vector3.back * Time.deltaTime * Speed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                IsFollow = false;
                CameraTf.position += Vector3.left * Time.deltaTime * Speed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                IsFollow = false;
                CameraTf.position += Vector3.right * Time.deltaTime * Speed;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Offset.z -= Input.GetAxis("Mouse ScrollWheel")* Time.deltaTime * ZoomSpeed;
               // CameraTf.position += CameraTf.forward * Time.deltaTime * Speed;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Offset.z += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
               // CameraTf.position += CameraTf.forward * Time.deltaTime * Speed;
            }

            if (IsFollow == true)
            {
                float dt = Time.deltaTime;
                Quaternion newrotation = Quaternion.Euler(RoationOffset);
                Vector3 newposition = newrotation * Offset + Target;

                CameraTf.rotation = Quaternion.Lerp(CameraTf.rotation, newrotation, dt * Damping);
                CameraTf.position = Vector3.Lerp(CameraTf.position, newposition, dt * Damping);
            }

            MouseMain();
        }

        private void MouseMain()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, Layer))
            {
                GameObject go = hit.collider.gameObject;
                var point = hit.point;

                UpdateMousePoint(point, go);
                UpdateLeftMouse(point, go);
                UpdateRightMouse(point, go);
            }
            else
            {

            }
        }



        public void Follow(Vector3 target)
        {
            IsFollow = true;
        }

        public void SetTarget(Vector3 target, bool isfollow = false)
        {
           // RtsCamera.LookAt = target;
            Target = target;
            IsFollow = isfollow;
        }

        public void SetTarget(Transform target)
        {
            RtsCamera.Follow(target,true);
        }

        /// <summary>
        /// 更新鼠标点位置
        /// </summary>
        public void UpdateMousePoint(Vector3 point, GameObject go)
        {
            MousePoint = point;
            MouseTarget = go;

        }

        public void UpdateLeftMouse(Vector3 point, GameObject go)
        {
            //左键点击
            if (Input.GetMouseButtonDown(0))
            {
                MouseInfo info = new MouseInfo();
                info.go = go;
                info.Point = point;

                MouseEventArgs eventArgs = new MouseEventArgs();
                eventArgs.MouseButton = 0;
                eventArgs.TouchPoint = Input.mousePosition;
                SingleStart?.Invoke(info, eventArgs);

            }
        }

        public void UpdateRightMouse(Vector3 point, GameObject go)
        {
            //右键点击
            if (Input.GetMouseButton(1))
            {
                MouseInfo info = new MouseInfo();
                info.go = go;
                info.Point = point;

                MouseEventArgs eventArgs = new MouseEventArgs();
                eventArgs.MouseButton = 1;
                eventArgs.TouchPoint = Input.mousePosition;
                SingleStart?.Invoke(info, eventArgs);
            }
        }





    }

}

