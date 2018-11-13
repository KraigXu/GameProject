using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WX
{
    public static class ComFun 
    {
        public static string GetRaceType(RaceType type)
        {
            switch (type)
            {
                case RaceType.Elf:
                    return "魂";
                case RaceType.Human:
                    return "人";
                default:
                    return "Wu";
            }
        }

        public static string GetSex(SexType type)
        {
            switch (type)
            {
                case SexType.Female:
                    return "女";
                case SexType.Male:
                    return "男";
                case SexType.Neutral:
                    return "中";
                default:
                    return "Wu";
            }
        }

        private const string _prestige1 = "无名小辈";
        private const string _prestige2 = "武林新秀";
        private const string _prestige3 = "世人皆知";
        private const string _prestige4 = "名扬天下";
        private const string _prestige5 = "传说";
        public static string GetPrestige(int value)
        {
            if (value >= 0 && value < 100)
            {
                return _prestige1;
            }
            else if(value >= 100 && value < 300)
            {
                return _prestige2;
            }else if (value >= 300 && value < 600)
            {
                return _prestige3;
            }else if (value >= 600 && value < 1000)
            {
                return _prestige4;
            }
            else if(value >=1000)
            {
                return _prestige5;
            }
            else
            {
                return _prestige1;
            }

        }

        public static string GetInfluence(int value)
        {
            return "";
        }

        public static string GetDisposition(int value)
        {
            return "";
        }
    }

}

