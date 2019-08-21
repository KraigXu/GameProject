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

        public LayerMask Layer;
        public Vector3 MousePoint;
        public GameObject MouseTarget;
        public GameObject TargetOj;

        public OverLookCameraController CameraController;


        void Awake()
        {
            _instance = this;
            CameraController = gameObject.GetComponent<OverLookCameraController>();
        }

        void Start()
        {
            //SetTarget(TargetOj.transform.position,true);
        }
        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

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

        }

        public void SetTarget(Vector3 target, bool isfollow = false)
        {
            CameraController.m_targetPosition = target;
        }

        public void SetDownClick(Vector3 target, bool isfollow = false)
        {

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

