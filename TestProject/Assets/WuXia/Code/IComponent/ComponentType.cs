using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WX
{
    public struct Mouse : IComponentData { }

    /// <summary>
    /// 可交互物
    /// </summary>
    public struct Interactable : IComponentData
    {
        public int InteractionType;
    }

    /// <summary>
    /// Npc
    /// </summary>
    public struct NpcInput : IComponentData
    {
        //趋向
        public int tend;
         
    }

    public enum TragetType {Idie,City, Field }
    

    public struct PlayerInput : IComponentData
    {

        public int PlayerCameraStatus;

    }
    public struct District : IComponentData
    {
        public int DistrictId;
        public int FactionId;
        public int GrowingModulus;
        public int SecurityModulus;
        public int Traffic;
        //public string Name;
        //public string Description;
        
        
        
        //public List<LivingArea> LivingAreaChilds;
        //public Projector Projector;
        //public Transform Model;
        //public Material Material;
    }

    public struct BiologicalText:IComponentData
    {
        public string Name;
    }
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

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;

        public int Jing;
        public float RestoreJing;
        public int Qi;
        public float RestoreQi;
        public int Shen;
        public float RestoreShen;

        public int WaigongMin;
        public int WaigongMax;
        public int NeigongMin;
        public int NeigongMax;

        public int StrategyMoveSpeed;
        public int FireMoveSpeed;
        public int ShanBi;

        public int LocationCode;
        public int LocationType;
        public int LocationStatus;

        

        
       // public int Huti;

        //public int GenGu;
        //public int LingMin;
        //public int DongCha;
        //public int JiYi;
        //public int WuXing;
        //public int YunQi;

        ////public DateTime TimeAppearance;
        ////public DateTime TimeEnd;

        
        
        //public int Magic;
        //public int CurMagic;
        //public int AttackOutMin;
        //public int AttackOutMax;
        //public int AttackInMin;
        //public int AttackInMax;

       
        //public int StatusCode;
        //public int GruopId;

        public Vector3 CurTarget;

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

    public struct LivingArea : IComponentData
    {

        public int Id;         
        public int PersonNumber;
        public int CurLevel;
        public int MaxLevel;
        public int TypeId;
        public int Money;
        public int MoneyMax;
        public int Iron;
        public int IronMax;
        public int Wood;
        public int WoodMax;
        public int Food;
        public int FoodMax;
        public int DefenseStrength;
        public int StableValue;
        public int Renown;

        public int IsInternal;   //是否显示内部 0 不显示 1显示

        //public BuildingObject[] BuildingObjects;
        //public string Name;
        //public string Description;

        //public GameObject LivingAreaM;
        //public Renderer LivingAreaRender;
        //public Collider LivingAreaCollider;
        //public bool IsOpen = false;  //是否打开

        //public LivingAreaState[] Groups = new LivingAreaState[0];

    }



}

