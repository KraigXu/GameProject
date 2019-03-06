using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlaySoundTrigger : SkillBehavior
    {

        public AudioData Data;
        public int AudioId;

        public override void Act(SkillInstance controller)
        {
        }

        public override ISkillTrigger Clone()
        {
            return new PlaySoundTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime &&m_IsExected==false)
            {
                Debug.Log("PlaySound");
                m_IsExected = true;
                WXAudioController.instance.ShowAudio(Vector3.up,Data.freed);
                return true;
            }
            return false;
        }

        public override void Init(string args)
        {
            string[] values = args.Split(',');

            m_StartTime = float.Parse(values[1]);
            AudioId = int.Parse(values[2]);
            Data = WXAudioController.instance.GetAudioData(AudioId);
        }

    }

}