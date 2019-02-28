using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlayAudioTrigger : SkillBehavior
    {
        public int AudioId;

        public override void Act(SkillInstance controller)
        {
            throw new System.NotImplementedException();
        }

        public override ISkillTrigger Clone()
        {
            throw new System.NotImplementedException();
        }

        public override bool Execute(ISkillTrigger instance, float curTime)
        {
            if (curTime >= m_StartTime)
            {
                WXAudioController.instance.ShowAudio(Vector3.down,AudioId);
                return true;
            }
            return false;
        }

        public override void Init(string args)
        {
            throw new System.NotImplementedException();
        }


    }

}