using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;
namespace WX
{
    public class Biological : MonoBehaviour
    {
        public int Id;
        public string Surname;
        public string Name;
        public string AvatarCode;
        public string ModeCode;
        public string Title;
        public string Description;
        public RaceType RaceType;
        public SexType Sex;
        public int Age;
        public int AgeMax;
        public int Prestige;
        public int Influence;
        public int Disposition;
        public DateTime TimeAppearance;
        public DateTime TimeEnd;
        public int Property1;
        public int Property2;
        public int Property3;
        public int Property4;
        public int Property5;
        public int Property6;
        public string FeatureIds;
        public string Location;
        public LocationType LocationType;
        public string ArticleJson;
        public string EquipmentJson;
        public string LanguageJson;
        public string GongfaJson;
        public string JifaJson;

        public int Blood;
        public int CurBlood;
        public int Magic;
        public int CurMagic;
        public int AttackOutMin;
        public int AttackOutMax;
        public int AttackInMin;
        public int AttackInMax;
        public BiologicalStatus CurStatus;
        public WhereStatus CurWhereStatus;
        public int GroupId = -1;                 //队伍ID
        public Sprite Avatar;

    }
}