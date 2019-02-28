﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WXDespawn : MonoBehaviour {

    public float DespawnDelay; // Despawn delay in ms
    public bool DespawnOnMouseUp; // Despawn on mouse up used for beams demo 

    AudioSource aSrc; // Cached audio source component

    void Awake()
    {
        // Get audio source component
        aSrc = GetComponent<AudioSource>();
    }

    // OnSpawned called by pool manager 
    public void OnSpawned()
    {
        // Invokes despawn using timer delay
        if (!DespawnOnMouseUp)
            WXTime.time.AddTimer(DespawnDelay, 1, DespawnOnTimer);
    }

    // OnDespawned called by pool manager 
    public void OnDespawned()
    {

    }

    // Run required checks for the looping audio source and despawn the game object
    public void DespawnOnTimer()
    {
        if (aSrc != null)
        {
            if (aSrc.loop)
                DespawnOnMouseUp = true;
            else
            {
                DespawnOnMouseUp = false;
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
        WXPoolManager.Pools["GeneratedPool"].Despawn(transform);
        //F3DPool.instance.Despawn(transform);
    }

    void Update()
    {
        // Despawn on mouse up        
        if (Input.GetMouseButtonUp(0))
            if (aSrc != null && aSrc.loop || DespawnOnMouseUp)
                Despawn();
    }
}
