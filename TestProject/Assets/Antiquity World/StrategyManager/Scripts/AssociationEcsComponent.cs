using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace AntiquityWorld.StrategyManager
{
    public class AssociationEcsComponent : MonoBehaviour
    {
        public Entity Entity;
        public bool IsAssociatino = false;

        void Start()
        {
        }

        void Update()
        {
            if (IsAssociatino == true)
            {

            }
        }

        public bool Association(Entity entity, Vector3 worldPosition)
        {
            try
            {
                Entity = entity;
                IsAssociatino = true;

                transform.position = worldPosition;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }



    }


}
