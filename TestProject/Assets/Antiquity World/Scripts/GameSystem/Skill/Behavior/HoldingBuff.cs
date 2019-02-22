using UnityEngine;
namespace GameSystem.Skill
{
    /// <summary>
    /// 圆形力场
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/Skill/SkillBehavior/HoldingBuff")]
    public class HoldingBuff : SkillBehavior
    {
        public GameObject  Effect;
        public GameObject HitEffect;
        [Range(1,10)]
        public float Range;

        public float Frequency=0.3f;           //频率

        private GameObject _currentEffect;
        private SkillController skillController;
        
        public override void Act(SkillController controller)
        {
            if (_currentEffect == null)
            {
                _currentEffect = GameObject.Instantiate(Effect, controller.transform);
                _currentEffect.transform.position = controller.transform.position;
                SFTime.Instance.AddTimer(Frequency, 10, Dot);
                skillController = controller;
            }
            
        }

        void Dot()
        {
            Ray ray = new Ray(skillController.transform.position, skillController.transform.up);
            RaycastHit[] hit = Physics.SphereCastAll(ray, Range, 10);
            Transform[] targets = new Transform[hit.Length];
            for (int i = 0; i < hit.Length; i++)
            {
                targets[i] = hit[i].collider.transform;

                GameObject go = Instantiate(HitEffect, targets[i]);
                go.transform.position = targets[i].position;
                
            }
            skillController.AllTarget = targets;
        }

        
    }

}

