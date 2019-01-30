using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{

    /// <summary>
    /// 元素类型
    /// </summary>
    public enum ElementType
    {
        None = 0,
        Terrain = 1,
        Biological = 2,
        District = 3,
        LivingArea = 4,
        Team= 5,
    }


    public enum TendType
    {
        None = 0, //无
        Patrol = 1,  //巡逻

    }

    public enum BehaviorPolicyType
    {
        Cruising,
    }

    public struct Element : IComponentData
    {
        public int InnerId;
        public ElementType Type;
    }

    /// <summary>
    /// 悬浮信息
    /// </summary>
    public struct FloatingInfo : IComponentData
    {

    }

    public struct BehaviorData : IComponentData
    {
        public Vector3 Target;
        public ElementType TargetType;
        public int TargetId;
        public float TimeToLive;
        public Entity TargetEntity;
    }

    public struct ModelSpawnData : IComponentData
    {
        public ModelComponent ModelData;
    }


    /// <summary>
    /// Npc
    /// </summary>
    public struct NpcInput : IComponentData
    {
        //趋向
        public TendType Movetend;

        public int RandomSeed;
        public BehaviorPolicyType BehaviorPolicy;
        public float BehaviorTime;
    }

    public struct PlayerInput : IComponentData
    {
        public Vector3 MousePoint;
        public Vector2 MousePosition;
        public Vector3 ClickPoint;
        public Vector2 ViewMove;
    }
    public struct District : IComponentData
    {
        public int Id;
        public int Type;
        public int ProsperityLevel;
        public int TrafficLevel;
        public int GrowingModulus;
        public int SecurityModulus;
        public int Value;
    }

    /// <summary>
    /// 区域标识  有此组件的实体会被标记为在大地图中
    /// </summary>
    public struct RegionTag : IComponentData
    {
        public Vector3 Position;                //位置
        public Quaternion Quaternion;           //角度
        public int DistrictId;                  //所处范围ID
    }

    public struct Biological : IComponentData
    {
        public int BiologicalId;


        public int AvatarId;
        public int ModelId;
        public int FamilyId;
        public int FactionId;
        public int TitleId;

        public int SexId;
        public int Age;
        public int AgeMax;
        public int Disposition;
        public int PrestigeValue;
        public int CharmValue;
        public byte CharacterValue;     //性格值
        public byte NeutralValue;       //中立值
        public byte BodyValue;         //身体值
        public byte LuckValue;          //运气值

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;

        public int Jing;
        public int Qi;
        public int Shen;
        public int WaigongMin;
        public int WaigongMax;
        public int NeigongMin;
        public int NeigongMax;

        public int Mingzhong;
        public int Shanbi;
        public int Huixin;

        public int StrategyMoveSpeed;
        public int FireMoveSpeed;

        public int TechniquesId;

        public int EquipmentId;
    }

    public struct ModelComponent: IComponentData
    {
        public float Speed;
        public Vector3 Target;
        public int Id;
        public int Status;
    }

    public struct Team : IComponentData
    {
        public int TeamBossId;
    }

    public enum TargetType { None, City, Field, Biological }
    //public struct BiologicalStatus : IComponentData
    //{
    //    public int BiologicalIdentity;          //身份编号 0  AI 1 玩家
    //    public LocationType LocationType;       //实时状态
    //    public int LocationId;                  //所处位置ID
    //    public float IdleTime;                  //闲置时间

    //    public int TargetId;                    // 目标ID
    //    public ElementType TargetType;          // 目标类型
    //    public Vector3 TargetPosition;          //目标位置
    //    public LocationType TargetLocationType;  //目标所处状态

    //    public Entity TargetEntity;
    //}
    public struct Family : IComponentData
    {
        public int FamilyId;
        public int ThisId;
    }

    public struct Faction : IComponentData
    {
        public int Id;
        public int Level;
        public int Type;

        public int Money;
        public int MoneyMax;
        public int Iron;
        public int IronMax;
        public int Wood;
        public int WoodMax;
        public int Food;
        public int FoodMax;
    }

    public struct Equipment : IComponentData
    {
        public int HelmetId;
        public int ClothesId;
        public int BeltId;
        public int HandGuard;
        public int Pants;
        public int Shoes;
        public int WeaponFirstId;
        public int WeaponSecondaryId;
    }

    public struct Techniques : IComponentData
    {
        public int Id;
        public int ParentId;
        public int BiologicalId;
        public int TechniquesValue;
        public int Effect;
    }

    public struct Wuxue : IComponentData
    {
        public int Value;
    }

    public enum LivingAreaType
    {
        SingleRoom=1,  //独房
        Sect=2,       //门派
        Village=3,    //村庄
        City=4,       //城市
    }

    public struct LivingArea : IComponentData
    {
        public int Id;
        public int PersonNumber;
        public LivingAreaType Type;
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

        public int PowerId;
        public int ModelBaseId;
        public int ModelId;
      
        public int TitleUiId;
        public int BuildGroupId;

        public int CurLevel;
        public int MaxLevel;
    }

    public struct LivingAreaInside : IComponentData
    {
        
    }



    public struct LivingAreaEnterInfo : IComponentData
    {
        public int UiType;

        public Entity LivingAreaEntity;
        public Entity TargetEntity;
    }

    public struct Building : IComponentData
    {
        public int LivingAreaId;
        public int BuildingModelId;

        public int Id;
        public int ParentId;
        public int Level;
        public int Status;
        public int Type;
        public int DurableValue;
        public int OwnId;
        public Vector3 Position;

        public int LoactionId;
        public int LoactionType;
    }

    public struct PrestigeValue : IComponentData
    {
        public int value;
    }

    /// <summary>
    /// 可交互物
    /// </summary>
    public struct InteractionElement : IComponentData
    {      
        public int Distance;
        public int ModelCode;

    }


    /// <summary>
    /// 两个Biological交互时信息体
    /// </summary>
    public struct InteractionBiologicalData
    {
        public int IsUi;
        public int BiologicalAId;
        public int BiologicalBId;
    }


    public struct Article : IComponentData
    {

    }

    public struct Relation : IComponentData
    {
        public int ObjectAid;
        public int ObjectBid;
        public int Value;

    }

    public struct Sound : IComponentData
    {
        public int SoundId;
    }

    /// <summary>
    /// 周期类型
    /// </summary>
    public enum PeriodType
    {
        Year, Month, Day, Shichen
    }

    /// <summary>
    /// 周期时间
    /// </summary>
    public struct PeriodTime : IComponentData
    {
        public byte Value;
        public PeriodType Type;
    }

    public struct EventInfo : IComponentData
    {
        public int Aid;           //为接触方
        public int Bid;          //被接触方
        public int EventCode;
    }

    public struct BiologicalSocial : IComponentData
    {
        public int Info;

    }


}

