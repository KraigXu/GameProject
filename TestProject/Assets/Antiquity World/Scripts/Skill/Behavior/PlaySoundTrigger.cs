using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlaySoundTrigger : SkillBehavior
    {

        public AudioData Data;
        public Transform asTf;
        public int AudioId;

        public override void Act(SkillInstance controller){}
        public override void Reset()
        {
            base.Reset();
            if (asTf != null)
            {
                bool flag = WXPoolManager.Pools[Define.PoolName].Despawn(asTf);
                Debug.Log("回收音效" + flag);
            }
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
                asTf = WXPoolManager.Pools[Define.PoolName].SpawnAudio(WXAudioController.instance.audioSource, Data.freed, Vector3.up, null);
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