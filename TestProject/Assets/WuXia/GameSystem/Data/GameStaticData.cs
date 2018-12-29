using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public sealed class GameStaticData
    {

        public static Dictionary<int, GameObject> ModelPrefab = new Dictionary<int, GameObject>();

        public static Dictionary<int, string> DistrictName = new Dictionary<int, string>();
        public static Dictionary<int, string> DistrictDescriptione = new Dictionary<int, string>();

        public static Dictionary<int, string> LivingAreaLevel = new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaType = new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaName = new Dictionary<int, string>();
        public static Dictionary<int, string> LivingAreaDescription = new Dictionary<int, string>();

        public static Dictionary<int, string> BuildingName = new Dictionary<int, string>();
        public static Dictionary<int, string> BuildingDescription = new Dictionary<int, string>();
        public static Dictionary<int, string> BuildingType = new Dictionary<int, string>();
        public static Dictionary<int, string> BuildingStatus = new Dictionary<int, string>();
        public static Dictionary<int, Sprite> BuildingAvatar = new Dictionary<int, Sprite>();

        public static Dictionary<int, string> FeaturesName = new Dictionary<int, string>();

        public static Dictionary<int, string> PrestigeTitle = new Dictionary<int, string>();

        public static Dictionary<int, string> BiologicalSex = new Dictionary<int, string>();
        public static Dictionary<int, string> BiologicalSurnameDic = new Dictionary<int, string>();
        public static Dictionary<int, string> BiologicalNameDic = new Dictionary<int, string>();
        public static Dictionary<int, string> BiologicalDescription = new Dictionary<int, string>();
        public static Dictionary<int, Sprite> BiologicalAvatar = new Dictionary<int, Sprite>();

        public static Dictionary<int, string> FactionName = new Dictionary<int, string>();
        public static Dictionary<int, string> FactionDescription = new Dictionary<int, string>();

        public static Dictionary<int, string> FamilyName = new Dictionary<int, string>();

        public static Dictionary<int, string> SocialDialogNarration = new Dictionary<int, string>();
        public static Dictionary<int, string> SocialDialogInfo = new Dictionary<int, string>();

        public static Dictionary<int, string> TimeJijie = new Dictionary<int, string>();
        //        子时 丑时  寅时 卯时  辰时 巳时
        //        23:00 - 00:59 01:00 - 02:59 03:00 - 04:59 05:00 - 06:59 07:00 - 08:59 09:00 - 10:59
        //        午时 未时  申时 酉时  戊时 亥时
        //        11:00 - 12:59 13:00 - 14:59 15:00 - 16:59 17:00 - 18:59 19:00 - 20:59 21:00 - 22:59
        public static Dictionary<int, string> TimeShichen = new Dictionary<int, string>();

        public static Dictionary<int,string> TechniquesName=new Dictionary<int, string>();
        public static Dictionary<int,string> TechniquesDescription=new Dictionary<int, string>();
        public static Dictionary<int,Sprite> TechniqueSprites=new Dictionary<int, Sprite>();

    }
}

