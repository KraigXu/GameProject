using System;
using Unity.Entities;

namespace District
{
    [Serializable]
    public struct District : IComponentData
    {
        public int Id;
       // public string Name;
        //public string Description;
        public int FactionId;
        public int GrowingModulus;
        public int SecurityModulus;
        public int Traffic;
        //public List<LivingAreaNode> LivingAreaChilds;
        //public Projector Projector;
        //public Transform Model;
        //public Material Material;
    }
    [UnityEngine.DisallowMultipleComponent]
    public class DistrictComponent : ComponentDataWrapper<District> { }


}

