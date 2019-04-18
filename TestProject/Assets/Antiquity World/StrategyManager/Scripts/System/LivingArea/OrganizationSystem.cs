using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 组织
    /// </summary>
    public class OrganizationSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObjects;
            public ComponentDataArray<LivingArea> LivingArea;
            public ComponentDataArray<Collective> Collective;
        }
        [Inject]
        private Data _data;

        protected override void OnUpdate()
        {

        }

        public static void OrganizationColliderEnter(GameObject go, Collider other)
        {

        }

        public static void OrganizationColliderExit(GameObject go, Collider other)
        {

        }
    }
}
