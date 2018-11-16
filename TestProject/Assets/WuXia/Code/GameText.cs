using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace WX
{
    public sealed class GameText
    {
        public static Dictionary<int,string> BiologicalSex=new Dictionary<int, string>();

        public static Dictionary<int,string> LivingAreaLevel=new Dictionary<int, string>();
        public static Dictionary<int,string> LivingAreaType=new Dictionary<int, string>();
        public static Dictionary<int,string> LivingAreaModelPath=new Dictionary<int, string>();

        public static Dictionary<int,string> PrestigeBiolgicalDic=new Dictionary<int, string>();
        public static Dictionary<int,string> PrestigeDistrictDic = new Dictionary<int, string>();
        public static Dictionary<int,string> PrestigeLivingAreaDic = new Dictionary<int, string>();

        public static Dictionary<Entity,string> SurnameDic=new Dictionary<Entity, string>();
        public static Dictionary<Entity,string> NameDic=new Dictionary<Entity, string>();
        public static Dictionary<Entity,string> Description=new Dictionary<Entity, string>();

        public static Dictionary<Entity,string> BuildingNameDic=new Dictionary<Entity, string>();
        public static Dictionary<Entity,string> BuildingDescriptionDic=new Dictionary<Entity, string>();
        public static Dictionary<int,string> BuildingType=new Dictionary<int, string>();
        public static Dictionary<int,string> BuildingStatus=new Dictionary<int, string>();

    }
}

