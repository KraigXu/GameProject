using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public sealed class GameStaticData
    {
        public static Dictionary<int, string> LivingAreaLevel = new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaType = new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaName = new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaDescription = new Dictionary<int, string>();
        public static Dictionary<int, GameObject> LivingAreaPrefabDic = new Dictionary<int, GameObject>();

        public static Dictionary<int, string> OrganizationName = new Dictionary<int, string>();
        public static Dictionary<int, string> OrganizationDescription = new Dictionary<int, string>();

        public static Dictionary<int, string> BuildingName = new Dictionary<int, string>();
        public static Dictionary<int, string> BuildingDescription = new Dictionary<int, string>();
        public static Dictionary<int, string> BuildingType = new Dictionary<int, string>();
        public static Dictionary<int, string> BuildingStatus = new Dictionary<int, string>();
        public static Dictionary<int, Sprite> BuildingAvatar = new Dictionary<int, Sprite>();

        public static Dictionary<int, string> FeaturesName = new Dictionary<int, string>();

        public static Dictionary<int, string> PrestigeTitle = new Dictionary<int, string>();

        public static Dictionary<Entity,CityRunData> CityRunDataDic=new Dictionary<Entity, CityRunData>();

        public static Dictionary<Entity, FactionStatic> FactionStatics= new Dictionary<Entity, FactionStatic>();
        public static Dictionary<int, string> FactionName = new Dictionary<int, string>();
        public static Dictionary<int, string> FactionDescription = new Dictionary<int, string>();
        public static Dictionary<int, string> FamilyName = new Dictionary<int, string>();
        public static Dictionary<int, string> SocialDialogNarration = new Dictionary<int, string>();
        public static Dictionary<int, string> SocialDialogInfo = new Dictionary<int, string>();

        public static Dictionary<int, string> TechniquesName = new Dictionary<int, string>();
        public static Dictionary<int, string> TechniquesDescription = new Dictionary<int, string>();
        public static Dictionary<int, Sprite> TechniqueSprites = new Dictionary<int, Sprite>();

        public static Dictionary<Entity, ArticleItemFixed> ArticleDictionary=new Dictionary<Entity, ArticleItemFixed>();

    }
}

