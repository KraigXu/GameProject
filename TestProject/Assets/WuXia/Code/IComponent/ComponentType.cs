using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WX
{
    public struct Mouse : IComponentData { }

    /// <summary>
    /// 可交互物
    /// </summary>
    public struct Interactable : IComponentData
    {
        public int InteractionType;
    }

    public struct PlayerInput : IComponentData
    {
        
        public float3 Move;
        public float3 Shoot;
        public float FireCooldown;

        public bool Fire => FireCooldown <= 0.0 && math.length(Shoot) > 0.5f;
    }

    public struct Player : IComponentData { }



    public struct District : IComponentData
    {
        public int DistrictId;
        public int FactionId;
        public int GrowingModulus;
        public int SecurityModulus;
        public int Traffic;
        //public string Name;
        //public string Description;
        
        
        
        //public List<LivingArea> LivingAreaChilds;
        //public Projector Projector;
        //public Transform Model;
        //public Material Material;
    }

    public struct LivingArea : IComponentData
    {

        public int Id;         
        public int PersonNumber;
        public int CurLevel;
        public int MaxLevel;
        public int TypeId;
        public int Money;
        public int MoneyMax;
        public int Iron;
        public int IronMax;
        public int Wood;
        public int WoodMax;
        public int Food;
        public int FoodMax;
        public int DefenseStrength;
        public int StableValue;
        public int Renown;

        public int IsInternal;   //是否显示内部 0 不显示 1显示

        //public BuildingObject[] BuildingObjects;
        //public string Name;
        //public string Description;

        //public GameObject LivingAreaM;
        //public Renderer LivingAreaRender;
        //public Collider LivingAreaCollider;
        //public bool IsOpen = false;  //是否打开

        //public LivingAreaState[] Groups = new LivingAreaState[0];

    }


}

