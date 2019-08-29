using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using GameSystem.Ui;

namespace GameSystem
{

    public delegate void EventCallBack(EventInfo info);

    public class GameEventSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<EventInfo> Info;
            public EntityArray Entity;
        }

        [Inject] private Data _data;

        public static Dictionary<int, EventCallBack> EventDic = new Dictionary<int, EventCallBack>();

        private EntityManager _entityManager;

        public GameEventSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        public static void SetEvent(int id, EventCallBack main)
        {
            if (EventDic.ContainsKey(id) == false)
            {
                EventDic.Add(id, main);
            }
            else
            {
                Debug.Log("事件Key{" + id + "}重复");
            }
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                EventInfo info = _data.Info[i];
                if (EventDic.ContainsKey(info.EventCode))
                {
                    EventDic[info.EventCode].Invoke(info);

                }
                _entityManager.DestroyEntity(_data.Entity[i]);
            }

        }
    }


}
