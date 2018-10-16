using System.Collections;
using System.Collections.Generic;
using GameSystem.AI;
using UnityEngine;
namespace GameSystem.Skill
{
    [CreateAssetMenu(menuName = "GameSystem/Skill/Behavior/RoundAOEBehavior")]
    public class RoundAOEBehavior : SkillBehavior
    {
        public GameObject EffectRangePrefab;
        [Range(3f, 10f)]
        public float radius = 5f;

        private GameObject _effect;

        public override void Act(SkillController controller)
        {
            if (_effect == null)
            {
             //   _effect = Instantiate(EffectRangePrefab, controller.transform, true);
              //  _effect.transform.localPosition = Vector3.zero;
               // _effect.GetComponent<RangeIndicator>().Scale = radius;

            //    CapsuleCollider capsuleCollider= controller.gameObject.AddComponent<CapsuleCollider>();
                //capsuleCollider.radius = radius/2f;
                //capsuleCollider.isTrigger = true;
                //controller.ColliderSelf = capsuleCollider;
            }

        }
    }
}

