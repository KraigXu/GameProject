using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class StrategyCameraManager : MonoBehaviour
    {
        public static StrategyCameraManager _instance;

        public static StrategyCameraManager Instance
        {
            get { return _instance; }
        }

        public Vector3 Target;
        public Vector3 RoationOffset= new Vector3(50, 0, 0);
        public Vector3 Offset= new Vector3(0, 1, -15);
        public float Damping = 3;

        public Camera Camera;
        public Transform CameraTf;
        public bool IsFollow;
        public float Speed=3.5f;

        void Awake()
        {
            _instance = this;

        }

        void Start()
        {
        }

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetKey(KeyCode.W))
            {
                IsFollow = false;
                CameraTf.position+=Vector3.forward*Time.deltaTime* Speed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                IsFollow = false;
                CameraTf.position+=Vector3.back*Time.deltaTime*Speed;
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

            if (IsFollow == true )
            {
                float dt = Time.deltaTime;
                Quaternion newrotation = Quaternion.Euler(RoationOffset);
                Vector3 newposition = newrotation * Offset + Target;

                CameraTf.rotation = Quaternion.Lerp(CameraTf.rotation, newrotation, dt * Damping);
                CameraTf.position = Vector3.Lerp(CameraTf.position, newposition, dt * Damping);
            }
        }

        public void Follow(Vector3 target)
        {
            IsFollow = true;
        }

        public void SetTarget(Vector3 target,bool isfollow=false)
        {
            Target = target;
            IsFollow = isfollow;
        }
    }

}

