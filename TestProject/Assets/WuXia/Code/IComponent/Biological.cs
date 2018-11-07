using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
namespace WX
{
    public struct Biological : IComponentData
    {
        public int BiologicalId;
        public int RaceId;
        public int SexId;
        public int Age;
        public int AgeMax;
        public int Prestige;
        public int Influence;
        public int Disposition;

        public int GenGu;
        public int LingMin;
        public int DongCha;
        public int JiYi;
        public int WuXing;
        public int YunQi;

        //public DateTime TimeAppearance;
        //public DateTime TimeEnd;

        public int Blood;
        public int CurBlood;
        public int Magic;
        public int CurMagic;
        public int AttackOutMin;
        public int AttackOutMax;
        public int AttackInMin;
        public int AttackInMax;

        public int LocationCode;
        public int StatusCode;
        public int GruopId;


        //public int Id;
        //public string Surname;
        //public string Name;
        //public string AvatarCode;
        //public string ModeCode;
        //public string Title;
        //public string Description;
        //public RaceType RaceType;
        //public SexType Sex;

        //public string FeatureIds;
        //public string Location;
        //public LocationType LocationType;
        //public string ArticleJson;
        //public string EquipmentJson;
        //public string LanguageJson;
        //public string GongfaJson;
        //public string JifaJson;

       
        //public BiologicalStatus CurStatus;
        //public WhereStatus CurWhereStatus;
        //public int GroupId = -1;                
        //public Sprite Avatar;
    }
}