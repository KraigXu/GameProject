using System;
using System.Collections;
using System.Collections.Generic;
using Invector;
using Invector.vCharacterController;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    public class FightingInitData
    {
        public int SiteType;
        public List<PersonData> Persons=new List<PersonData>();
    }


    public class PersonData
    {
        public string Name;
        public int Life;
        public int Attack;
        public int Energy;
    }

    [Serializable]
    public class SkillData
    {
        public int Id;
        public Transform Prefab;
    }

    [Serializable]
    public class AudioData
    {
        public int Id;
        public AudioClip Clip;
    }

    public class FightingScene : MonoBehaviour
    {
        public static EntityArchetype FightingPerson;

        private static FightingScene _instance;
        public static FightingScene Instance
        {
            get { return _instance; }
        }

        public int PlayerId;
        public GameObject PlayerGo;
        public Camera PlayerCamera;
        public List<Transform> Enemy=new List<Transform>();

        public List<SkillData> SkillDatas = new List<SkillData>();

        public List<AudioData> AudioDatas = new List<AudioData>();
        public Transform audioSource;
        public vHUDController controller;
        void Awake()
        {
            _instance = this;

        }

        public AudioData GetAudioData(int id)
        {
            for (int i = 0; i < AudioDatas.Count; i++)
            {
                if (AudioDatas[i].Id == id)
                {
                    return AudioDatas[i];
                }
            }

            return AudioDatas[0];
        }

        public SkillData GetSkillData(int id)
        {
            for (int i = 0; i < SkillDatas.Count; i++)
            {
                if (SkillDatas[i].Id == id)
                {
                    return SkillDatas[i];
                }
            }

            return SkillDatas[0];
        }

        public void ShowUI(vDamage damage)
        {
            controller.DamageUi(damage);
        }
    }
}
