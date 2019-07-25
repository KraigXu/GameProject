using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;

public class BiologicalComponent : ComponentDataWrapper<Biological> {
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnEnable()
    {

        base.OnEnable();
       
    }

    protected override void ValidateSerializedData(ref Biological serializedData)
    {
        base.ValidateSerializedData(ref serializedData);
    }
}
