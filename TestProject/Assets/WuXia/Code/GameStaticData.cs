using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace WX
{
    public sealed class GameStaticData
    {
        public static Dictionary<int,string> BiologicalSex=new Dictionary<int, string>();

        public static Dictionary<int,string> LivingAreaLevel=new Dictionary<int, string>();
        public static Dictionary<int,string> LivingAreaType=new Dictionary<int, string>();
        public static Dictionary<int,string> LivingAreaModelPath=new Dictionary<int, string>();

        public static Dictionary<int,string> PrestigeBiolgicalDic=new Dictionary<int, string>();
        public static Dictionary<int,string> PrestigeDistrictDic = new Dictionary<int, string>();
        public static Dictionary<int,string> PrestigeLivingAreaDic = new Dictionary<int, string>();

        
        public static Dictionary<int, string> DistrictName = new Dictionary<int, string>();
        public static Dictionary<int, string> DistrictDescriptione = new Dictionary<int, string>();

        public static Dictionary<int,string> LivingAreaName=new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaDescriptione = new Dictionary<int, string>();

        public static Dictionary<int, string> BiologicalSurnameDic = new Dictionary<int, string>();
        public static Dictionary<int, string> BiologicalNameDic = new Dictionary<int, string>();
        public static Dictionary<int, string> BiologicalDescription = new Dictionary<int, string>();

        public static Dictionary<Entity,string> BuildingNameDic=new Dictionary<Entity, string>();
        
        public static Dictionary<Entity,string> BuildingDescriptionDic=new Dictionary<Entity, string>();
        public static Dictionary<int,string> BuildingType=new Dictionary<int, string>();
        public static Dictionary<int,string> BuildingStatus=new Dictionary<int, string>();

        public static Dictionary<int,Sprite> BiologicalAvatar=new Dictionary<int, Sprite>();
        public static Dictionary<int,Sprite> BuildingAvatar=new Dictionary<int, Sprite>();


        public static Dictionary<int,string> FactionName=new Dictionary<int, string>();


        public static Dictionary<int,string> FamilyName=new Dictionary<int, string>();
    }
}

