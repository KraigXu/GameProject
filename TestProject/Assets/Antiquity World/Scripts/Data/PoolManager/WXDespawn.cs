using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池回收组件
/// </summary>
public class WXDespawn : MonoBehaviour
{
    public float DespawnDelay; // Despawn delay in ms
    private AudioSource aSrc; // Cached audio source component

    void Awake()
    {
        // Get audio source component
        aSrc = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    // OnSpawned called by pool manager 
    void OnSpawned()
    {
        Debug.Log(DespawnDelay);
        WXTime.time.AddTimer(DespawnDelay, 1, DespawnOnTimer);
    }

    void OnDespawned()
    {
    }

    // Run required checks for the looping audio source and despawn the game object
    public void DespawnOnTimer()
    {
       
        if (aSrc != null)
        {
            if (aSrc.loop)
            {

            }
            else
            {
                Despawn();
            }
        }
        else
        {
            Despawn();
        }
    }

    // Despawn game object this script attached to
    public void Despawn()
    {
        WXPoolManager.Pools[Define.PoolName].Despawn(transform);
    }

    void Update()
    {

    }

    void OnDestroy()
    {
    }
}
