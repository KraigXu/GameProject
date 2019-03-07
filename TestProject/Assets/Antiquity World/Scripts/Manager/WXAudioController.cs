using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class AudioData
{
    public int Id;
    public AudioClip[] Hit;
    public AudioClip freed;
    public float freedDelay;
    public float hitDelay;
}

public class WXAudioController : MonoBehaviour
{
    // Singleton instance 
    public static WXAudioController instance;

    public List<AudioData> AudioDatas = new List<AudioData>();

    // Audio timers
    float timer_01, timer_02;
    public Transform audioSource;

    void Awake()
    {
        // Initialize singleton
        instance = this;
    }

    void Update()
    {
        timer_01 += Time.deltaTime;
        timer_02 += Time.deltaTime;
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

    public void ShowAudio(Vector3 pos, int id)
    {
        for (int i = 0; i < AudioDatas.Count; i++)
        {
            if (AudioDatas[i].Id == id)
            {
                if (timer_01 >= AudioDatas[i].freedDelay)
                {

                    AudioSource asrc = WXPoolManager.Pools[Define.PoolName].SpawnAudio(audioSource, AudioDatas[i].freed, pos, null).gameObject.GetComponent<AudioSource>();

                    if (asrc != null)
                    {
                        asrc.pitch = Random.Range(0.95f, 1f);
                        asrc.volume = Random.Range(0.8f, 1f);
                        asrc.minDistance = 5f;
                        asrc.loop = false;
                        asrc.Play();

                        timer_01 = 0f;
                    }
                }
                return;
            }
        }
    }

    public void ShowAudio(Vector3 pos, AudioClip clip)
    {
        AudioSource aSrc =WXPoolManager.Pools[Define.PoolName].SpawnAudio(audioSource, clip, pos, null).gameObject.GetComponent<AudioSource>();

        if (aSrc != null)
        {
            // Modify audio source settings specific to it's type
            aSrc.pitch = Random.Range(0.95f, 1f);
            aSrc.volume = Random.Range(0.8f, 1f);
            aSrc.minDistance = 5f;
            aSrc.loop = false;
            aSrc.Play();

            // Reset delay timer
            timer_01 = 0f;
        }
    }


}
