using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;

public class Biological : MonoBehaviour
{
    public BiologicalData Model;

    public int BiologicalId;
    public string Name;
    public string Description;
    public RaceType RaceType;
    public SexType Sex;
    public int Age;
    public int AgeMax;
    public DateTime TimeAppearance;
    public DateTime TimeEnd;

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
    public int GroupId=-1;                 //队伍ID
    

    public int RaceId { get; set; }
    public int RaceRangeId { get; set; }
    public int PowerId { get; set; }
    public int Life { get; set; }
    public int LifeMax { get; set; }
    public int Prestige { get; set; }
   public int IsDebut { get; set; }
    public int Location { get; set; }
    public int LocationType { get; set; }
    public string ArticleJson { get; set; }
    public string EquipmentJson { get; set; }
    public string LanguageJson { get; set; }
    private void Start()
    {
        if (Model != null)
        {
        }
    }

    private void Update()
    {

    }

    
}
