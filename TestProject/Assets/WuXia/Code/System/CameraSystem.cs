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
        struct CameraGroup
        {
            public readonly int Length;
            public ComponentDataArray<CameraProperty> Property;
        }
        [Inject]
        private CameraGroup _cameraGroup;


        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            _camera=Camera.main;
        }

        private Camera _camera;


        protected override void OnUpdate()
        {
            if (_camera == null)
            {
                _camera = StrategySceneInit.Settings.MainCamera;
            }

            for (int i = 0; i < _cameraGroup.Length; i++)
            {
                var position = _cameraGroup.Property[i];
                float dt = Time.deltaTime;
                Quaternion newrotation = Quaternion.Euler(position.RoationOffset);
                Vector3 newposition = newrotation * position.Offset + position.Target;
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, newrotation, dt * position.Damping);
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, newposition, dt * position.Damping);
            }
        }
    }


}
