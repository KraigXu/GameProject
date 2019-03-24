using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace AntiquityWorld.StrategyManager
{
    public class BiologicalBehaviourComponent : AssociationEcsComponent
    {

        private Animator _animator;
        private Collider _collider;
        private AICharacterControl _character;
        private ThirdPersonCharacter _thirdPersonCharacter;
        void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
            _collider = gameObject.GetComponent<Collider>();
            _character = gameObject.GetComponent<AICharacterControl>();
            _thirdPersonCharacter = gameObject.GetComponent<ThirdPersonCharacter>();
        }

        void Update()
        {
            if(IsAssociatino==false)
                return;

            BehaviorData target = SystemManager.GetProperty<BehaviorData>(Entity);

            if (target.Target != Vector3.zero)
            {
               _character.SetTarget(target.Target);
            }
        }
        void OnCollisionEnter(Collision collision)
        {
            AssociationEcsComponent component = collision.gameObject.GetComponent<AssociationEcsComponent>();
            if (component)
            {
                BehaviorData target = SystemManager.GetProperty<BehaviorData>(Entity);
                
                if (target.TargetEntity == component.Entity)
                {
                    Element selftype = SystemManager.GetProperty<Element>(Entity);
                    //触发判定
                    if (selftype.Type == ElementType.Biological && target.TargetType==ElementType.LivingArea)
                    {
                        SystemManager.Get<LivingAreaSystem>().LivingAreaEntityCheck(Entity, component.Entity);
                        //BiologicalSystem.
                    }

                }


            }

        }

        void OnCollisionExit(Collision collision)
        {

        }

        
    }

}