using System.Collections;
using System.Collections.Generic;

using GameSystem;
using Unity.Entities;
using UnityEngine;


/// <summary>
/// 势力系统
/// </summary>
public class CampSystem : ComponentSystem
{

    struct Data
    {
        public readonly int Length;
        public Camp Camps;
    }

    private EntityManager _entityManager;



    protected override void OnUpdate()
    {

    }


    public void AddCamp(CampData camp)
    {



        Entity entity = _entityManager.CreateEntity();


        _entityManager.AddComponentData(entity,new Camp
        {
            UniqueCode = 10,
        });



    }
}
