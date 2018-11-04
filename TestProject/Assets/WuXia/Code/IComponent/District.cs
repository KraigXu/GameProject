using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Strategy
{
    public class District : MonoBehaviour
    {
        public int Id;
        public string Name;
        public string Description;
        public int FactionId;
        public int GrowingModulus;
        public int SecurityModulus;
        public int Traffic;
       public List<LivingArea> LivingAreaChilds;
        public Projector Projector;
        public Transform Model;
        public Material Material;
    }


}

