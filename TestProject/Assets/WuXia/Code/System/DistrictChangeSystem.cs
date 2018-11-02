using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class DistrictChangeSystem : ComponentSystem {

    ComponentGroup m_MainGroup;

    protected override void OnCreateManager()
    {
        Debuger.Log(">>>");
       // m_MainGroup = GetComponentGroup(typeof(District), typeof(Position));
    }

    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
