using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class WXPoolManager : MonoBehaviour
{
    public static WXPoolManager instance;                                              // instance of current manager 
    public static Dictionary<string, WXPool> Pools = new Dictionary<string, WXPool>();

    public string databaseName = "";                                                    // Name of database. Need for editor 
    public int selectedItem = 0;                                                        // Editor's parameter 
    public bool[] haveToShowArr;                                                        // Editor's parameter               

    List<WXPool> curPools = new List<WXPool>();                                       // Our pools 

    void Awake()
    {
        InstallManager();
        instance = this;
    }

    //Retirning pool by it's name
    public WXPool GetPool(string valueName)
    {
        int i;
        if (valueName != "" && curPools != null && curPools.Count > 0)
        {
            for (i = 0; i < curPools.Count; i++)
            {
                if (curPools[i].poolName == valueName)
                {
                    return curPools[i];
                }
            }
        }
        return null;
    }

    //Installing of manager
    void InstallManager()
    {
        curPools.Clear();
        List<WXPoolContainer> pools = new List<WXPoolContainer>();
        Pools.Clear();
        Pools = new Dictionary<string, WXPool>();
        WXPoolManagerDB myManager = Resources.Load("PoolManagerCache/" + databaseName) as WXPoolManagerDB;
        if (myManager != null)
        {
            if (myManager.pools != null)
            {
                foreach (WXPoolContainer cont in myManager.pools)
                {
                    pools.Add(cont);
                }
            }
        }

        //Transfering info from containers to our real pools and creating GO's for them
        int j;
        for (j = 0; j < pools.Count; j++)
        {
            GameObject newGO = new GameObject();
            newGO.transform.parent = this.gameObject.transform;
            newGO.name = pools[j].poolName;

            WXPool newPool = newGO.AddComponent<WXPool>();
            newPool.poolName = newGO.name;
            newPool.SetTemplates(pools[j].templates);
            newPool.SetLength(pools[j].poolLength);
            newPool.SetLengthMax(pools[j].poolLengthMax);
            newPool.needBroadcasting = pools[j].needBroadcasting;
            newPool.broadcastSpawnName = pools[j].broadcastSpawnName;
            newPool.broadcastDespawnName = pools[j].broadcastDespawnName;
            newPool.needSort = pools[j].needSort;
            newPool.delayedSpawnInInstall = pools[j].delayedSpawnInInstall;
            newPool.objectsPerUpdate = pools[j].objectsPerUpdate;
            newPool.optimizeSpawn = pools[j].optimizeSpawn;
            newPool.targetFPS = pools[j].targetFPS;
            newPool.needSort = pools[j].needSort;
            newPool.needParenting = pools[j].needParenting;
            newPool.needDebugging = pools[j].needDebug;
            newPool.pooling = true;
            newPool.Install();

            curPools.Add(newPool);
            Pools.Add(newPool.name, newPool);

        }
    }

    public int GetPoolsCount()
    {
        if (curPools != null)
            return curPools.Count;
        return -1;
    }
}
