using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Skill
{
    public class PlaySoundTrigger : SkillBehavior
    {
        public AudioData Data;
        public AudioSource asrc;
        public int AudioId;
        public override ISkillTrigger Clone()
        {
            return new PlaySoundTrigger();
        }

        public override bool Execute(ISkillTrigger instance, float curTime, SkillInstance controller)
        {
            if (curTime >= m_StartTime && m_IsExected == false)
            {
                Debug.Log("PlaySound");
                m_IsExected = true;
                asrc = WXPoolManager.Pools[Define.PoolName].SpawnAudio(FightingScene.Instance.audioSource, Data.Clip, Vector3.up, null).GetComponent<AudioSource>();
                if (asrc != null)
                {
                    asrc.pitch = Random.Range(0.95f, 1f);
                    asrc.volume = Random.Range(0.8f, 1f);
                    asrc.minDistance = 5f;
                    asrc.loop = false;
                    asrc.Play();
                }
                return true;
            }
            return false;
        }

        public override void Init(string args)
        {
            string[] values = args.Split(',');

            m_StartTime = float.Parse(values[1]);
            AudioId = int.Parse(values[2]);
            Data = FightingScene.Instance.GetAudioData(AudioId);
        }

    }

}