using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Invector;
using Invector.vCharacterController;
using Unity.Entities;
using UnityEngine;

namespace  AntiquityWorld
{
    public class FightingInitData
    {
        public int SiteType;
        public List<PersonData> Persons = new List<PersonData>();
    }


    public class PersonData
    {
        public string Name;
        public int Life;
        public int Attack;
        public int Energy;
    }



    [Serializable]
    public class AudioData
    {
        public int Id;
        public AudioClip Clip;
    }

    [Serializable]
    public class CurveData
    {
        public int Id;
        public float Time;
        public AnimationCurve X = new AnimationCurve();
        public AnimationCurve Y = new AnimationCurve();
        public AnimationCurve Z = new AnimationCurve();
    }

    public class FightingController : MonoBehaviour
    {

        public static EntityArchetype FightingPerson;


        public int PlayerId;
        public GameObject PlayerGo;
        public Camera PlayerCamera;
        public List<Transform> Enemy = new List<Transform>();
        public List<ParticleItem> ParticleDatas = new List<ParticleItem>();
        public List<AudioData> AudioDatas = new List<AudioData>();
        public List<CurveData> CurveDatas = new List<CurveData>();
        public Transform audioSource;
        public vHUDController controller;

        private static FightingController _instance;

        public static FightingController Instance
        {
            get { return _instance; }
        }

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

        public ParticleItem GetSkillData(int id)
        {
            for (int i = 0; i < ParticleDatas.Count; i++)
            {
                if (ParticleDatas[i].Id == id)
                {
                    return ParticleDatas[i];
                }
            }

            return ParticleDatas[0];
        }

        public void ShowUi(vDamage damage)
        {
            controller.DamageUi(damage);
        }


    }


}
