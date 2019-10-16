using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public sealed class SystemManager
{
    private static SystemManager _instance;
    private static object _lock = new object();

    private SystemManager() { }

    private EntityManager _curEManager;

    public static EntityManager ActiveManager
    {
        get
        {
            if (GetInstance()._curEManager == null)
            {
                GetInstance()._curEManager= World.Active.GetOrCreateManager<EntityManager>();
            }

            return GetInstance()._curEManager;
        }
    }

    public static SystemManager GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new SystemManager();
                }
            }
        }
        return _instance;
    }
    /// <summary>
    /// 获取指定系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>() where T : ScriptBehaviourManager
    {
        return World.Active.GetExistingManager<T>();
    }

    /// <summary>
    /// 获取实体属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static T GetProperty<T>(Entity entity) where T : struct, IComponentData
    {
        return World.Active.GetOrCreateManager<EntityManager>().GetComponentData<T>(entity);
    }

    /// <summary>
    /// 判断实体内是否存在属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool Contains<T>(Entity entity) where T : struct, IComponentData
    {
        return World.Active.GetOrCreateManager<EntityManager>().HasComponent<T>(entity);
    }


    public static void AddProperty<T>(Entity entity,T t) where T : struct, IComponentData
    {
        return; World.Active.GetOrCreateManager<EntityManager>().AddComponentData(entity,t);
    }


}
