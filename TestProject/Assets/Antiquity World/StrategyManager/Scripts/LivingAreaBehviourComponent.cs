using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Transforms;
using UnityEngine;

namespace AntiquityWorld.StrategyManager
{
    public class LivingAreaBehviourComponent : AssociationEcsComponent
    {

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (IsAssociatino == false)
                return;

            Position position = SystemManager.GetProperty<Position>(Entity);
            transform.position = position.Value;
        }
    }
}
