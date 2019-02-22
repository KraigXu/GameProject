using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SystemManager
{


    private static EntityManager _instance;

    

    public static T Get<T>() where T : ScriptBehaviourManager
    {
        return World.Active.GetExistingManager<T>();
    }

    public static T GetProperty<T>(Entity entity) where T : struct, IComponentData
    {
        return World.Active.GetOrCreateManager<EntityManager>().GetComponentData<T>(entity);
    }

    public static bool Contains<T>(Entity entity) where T : struct, IComponentData
    {
        return World.Active.GetOrCreateManager<EntityManager>().HasComponent<T>(entity);
    }

}
