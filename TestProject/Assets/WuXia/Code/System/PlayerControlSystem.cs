using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using WX;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;

namespace  WX
{
    public class PlayerControlSystem : ComponentSystem
    {
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
            Debug.Log(">?>");
            base.OnStartRunning();
        }


        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;

            foreach (var entity in GetEntities<PlayerData>())
            {
                var pi = entity.Input;

                pi.Move.x = Input.GetAxis("Horizontal");
                pi.Move.y = Input.GetAxis("Vertical");
                pi.Shoot.x = Input.GetAxis("ShootX");
                pi.Shoot.y = Input.GetAxis("ShootY");

                pi.FireCooldown = Mathf.Max(0.0f, pi.FireCooldown - dt);
            }
        }
    }
}

