using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SystemManager  {


    public static T Get<T>() where T : ScriptBehaviourManager
    {
        return World.Active.GetExistingManager<T>();
    }

}
