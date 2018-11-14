using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace WX
{

    /// <summary>
    /// 3D相机管理
    /// </summary>
    public class CameraSystem : ComponentSystem
    {

        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Player;
            public ComponentArray<Transform> Position;
        }
        [Inject]
        private PlayerData _player;

        private int _curModel;

        struct LivingAreaData
        {
            public readonly int Length;
        }

        struct CamerData
        {
            public PlayerCamera camera;
        }

        struct CameraModelData
        {
            public ModelCamera camera;
        }


        protected override void OnUpdate()
        {
            

            for (int i = 0; i < _player.Length; i++)
            {
                var playerInput = _player.Player[i];

                if (playerInput.PlayerCameraStatus!= _curModel)
                {
                    _curModel = playerInput.PlayerCameraStatus;
                    
                    //if (_curModel == 0)
                    //{

                    //}
                    //else
                    //{

                    //}

                }

                if (_curModel == 0)
                {
                    var playerPosition = _player.Position[i].position;

                    foreach (var _camera in GetEntities<CamerData>())
                    {
                        float dt = Time.deltaTime;
                        Quaternion newrotation = Quaternion.Euler(_camera.camera.RoationOffset);
                        Vector3 newposition = newrotation * _camera.camera.Offset + new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
                        _camera.camera.transform.rotation = Quaternion.Lerp(_camera.camera.transform.rotation, newrotation, dt * _camera.camera.Damping);
                        _camera.camera.transform.position = Vector3.Lerp(_camera.camera.transform.position, newposition, dt * _camera.camera.Damping);
                    }
                }else if (_curModel == 1)
                {
                    foreach (var camera in GetEntities<CameraModelData>())
                    {
                        
                    }
                }

            }


        }



        //private void CamerMove(Transform playerTf)
        //{
        //    Vector3 _target = Vector3.zero;
        //    Camera _camera = Camera.main;
        //    Transform _cameraTf = _camera.GetComponent<Transform>();
        //    if (playerTf != null)
        //    {
        //        _target = playerTf.position;
        //    }

        //    if (Input.GetMouseButton(0))
        //    {
        //        _isManual = true;
        //    }
        //    else
        //    {
        //        _isManual = false;
        //    }


        //    if (_isManual)              //手动时以目标为中心
        //    {
        //        if (Input.GetMouseButton(1))
        //        {
        //            if (_lastMousePosition != Vector3.zero)
        //            {
        //                Vector2 offset = Input.mousePosition - _lastMousePosition;
        //                _cameraTf.RotateAround(_target, Vector3.up, offset.x * Time.deltaTime * _rotateSpeed);
        //                _cameraTf.RotateAround(_target, _cameraTf.right, -offset.y * Time.deltaTime * _rotateSpeed);
        //            }
        //            _lastMousePosition = Input.mousePosition;
        //        }

        //        if (Input.GetMouseButtonUp(1))
        //        {
        //            _lastMousePosition = Vector3.zero;
        //        }

        //        if (Input.GetMouseButton(2))
        //        {
        //            if (_lastMousePosition != Vector3.zero)
        //            {
        //                // float moveDistance = Time.deltaTime * -panSpeed * distance / 20f;
        //                Vector3 offset = Input.mousePosition - _lastMousePosition;
        //                _cameraTf.Translate(new Vector3(offset.x * Time.deltaTime * _moveSpeed, offset.y * Time.deltaTime * _moveSpeed, offset.z * Time.deltaTime * _moveSpeed));
        //            }
        //            _lastMousePosition = Input.mousePosition;
        //        }

        //        if (Input.GetMouseButtonUp(2))
        //        {
        //            _lastMousePosition = Vector3.zero;
        //        }

        //        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        //        {
        //            float moveValue = Input.GetAxis("Mouse ScrollWheel");
        //            float moveDistance = moveValue * Time.deltaTime * _zoomSpeed;
        //            _cameraTf.Translate(_cameraTf.forward * moveDistance);
        //        }

        //        if (Input.GetKey(KeyCode.W))
        //        {
        //            Vector3 offset = new Vector3(0, 0, Time.deltaTime * _moveSpeed);
        //            _cameraTf.Translate(offset, Space.World);
        //        }

        //        if (Input.GetKey(KeyCode.S))
        //        {
        //            Vector3 offset = new Vector3(0, 0, -Time.deltaTime * _moveSpeed);
        //            _cameraTf.Translate(offset, Space.World);
        //        }

        //        if (Input.GetKey(KeyCode.A))
        //        {
        //            Vector3 offset = new Vector3(-Time.deltaTime * _moveSpeed, 0, 0);
        //            _cameraTf.Translate(offset, Space.World);
        //        }

        //        if (Input.GetKey(KeyCode.D))
        //        {
        //            Vector3 offset = new Vector3(Time.deltaTime * _moveSpeed, 0, 0);
        //            _cameraTf.Translate(offset, Space.World);
        //        }

        //    }
        //    else                    //跟随目标
        //    {
        //        Quaternion newrotation = Quaternion.Euler(60f, 30f, 0);
        //        Vector3 newposition = newrotation * new Vector3(0, 0, -50) + _target;
        //        _cameraTf.rotation = Quaternion.Lerp(_cameraTf.rotation, newrotation, Time.deltaTime * _damping);
        //        _cameraTf.position = Vector3.Lerp(_cameraTf.position, newposition, Time.deltaTime * _damping);
        //    }
        //    //ChangeCamerStatus
        //    _camera.nearClipPlane = _originalNearPlane;
        //    _camera.farClipPlane = _originalFarPlane;
        //}

    }


}
