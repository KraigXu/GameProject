using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem
{
    /// <summary>
    /// 玩家输入
    /// </summary>
    public class PlayerInputSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Input;
            public EntityArray Entitys;
        }

        [Inject]
        private Data _data;



        protected override void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;


            for (int i = 0; i < _data.Length; i++)
            {
                var input = new PlayerInput();
                input.MousePosition = Input.mousePosition;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    input.MousePoint = hit.point;

                    if (Input.GetMouseButton(0))
                    {
                        input.ClickPoint = hit.point;
                    }
                    else
                    {
                        input.ClickPoint = Vector3.zero;
                    }

                    GameObjectEntity go = hit.collider.GetComponent<GameObjectEntity>();
                    if (go != null)
                    {
                        input.MouseEntity = go.Entity;
                    }

                }
                else
                {
                    input.MousePoint = Vector3.zero;
                    input.MouseEntity=Entity.Null;
                }


                Vector2 keyv = Vector2.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    keyv.y += Time.deltaTime * GameSceneInit.KeySpeed;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    keyv.y -= Time.deltaTime * GameSceneInit.KeySpeed;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    keyv.x -= Time.deltaTime * GameSceneInit.KeySpeed;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    keyv.x += Time.deltaTime * GameSceneInit.KeySpeed;
                }

                input.ViewMove = keyv;
                
                _data.Input[i] = input;
            }
        }

    }
}
