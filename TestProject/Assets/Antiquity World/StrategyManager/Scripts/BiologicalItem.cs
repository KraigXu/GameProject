using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace AntiquityWorld.StrategyManager
{
    public class BiologicalItem : MonoBehaviour
    {
        public Entity Entity;
        public Animator Animator;
        public Collider m_collider;
        public AICharacterControl _Character;
        public ThirdPersonCharacter _ThirdPersonCharacter;
        public bool IsInit = false;
        public Mesh _Mesh;

        void Start()
        {
            
        }

        void Update()
        {
            //if (IsInit == true)
            //{
            //    Biological biological = SystemManager.GetProperty<Biological>(Entity);

            //    Debug.Log(biological.Age);
            //}
        }
    }


}
