using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    [CreateAssetMenu(menuName ="GameSystem/Skill/SkillGroup")]
    public class SkillGroup : ScriptableObject
    {
        public int Id;                                      //编号
        public string Name;                            //名称
        public string Description;                   //说明
        public float CoolingTime;                  //冷却
        public Sprite Icon;                          //图标
        public Color sceneGizmoColor = Color.gray;      //拿来渲染eyes的Gizmos颜色
        public SkillBehavior[] behaviors;
        public void UpdateBehaviors(SkillController controller)
        {
            DoBehaviors(controller);
        }

        /// <summary>
        /// 顺序执行所有效果
        /// </summary>
        /// <param name="controller"></param>
        private void DoBehaviors(SkillController controller)
        {
            for (int i = 0; i < behaviors.Length; i++)
            {
                behaviors[i].Act(controller);
            }
        }

        

    }

}

