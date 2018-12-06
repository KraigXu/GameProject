using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GameSystem
{

    /// <summary>
    /// 元素类型
    /// </summary>
    public enum ElementType
    {
        None=0,
        Terrain=1,
        Biological=2,
        District=3,
        LivingArea=4
    }

    public struct CameraProperty : IComponentData
    {
        public Vector3 Target;
        public int Damping;
        public Vector3 Offset;
        public Vector3 RoationOffset;
    }

    public struct TimeData : IComponentData
    {
        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Shichen;
        public int Jijie;

        public byte TimeScalar;           //时间的放大比 如果是0 则是暂停
        public float Schedule;             //一个时间节点的进度
        public byte ScheduleCell;         //时间节点的大小
    }


    public enum TendType
    {
        None=0, //无
        Patrol=1,  //巡逻

    }

    public enum BehaviorPolicyType
    {
        Cruising,

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

    }
    public struct District : IComponentData
    {

        public int Id;
        public int Type;
        public int ProsperityLevel;
        public int TrafficLevel;
        public int GrowingModulus;
        public int SecurityModulus;
    }


    public struct Biological : IComponentData
    {
        public int BiologicalId;
        public int AvatarId; 
        public int ModelId;
        public int PrestigeId;
        public int RelationId; 
        public int FamilyId;
        public int FactionId;
        public int TitleId;

        public int SexId;
        public int Age;
        public int AgeMax;
        public int Disposition;


        //固有属性
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


        //性格属性
        public byte CharacterValue;
        public byte NeutralValue; //中立值

    }

    public enum TargetType { None, City, Field, Biological }
    public struct BiologicalStatus : IComponentData
    {
        public Vector3 Position;        //  位置
        public int TargetId;            // 目标ID
        public ElementType TargetType;          // 目标类型
        public Vector3 TargetPosition;       //目标位置

        public LocationType LocationType;       // 实时状态
        public int LocationId;           //所处位置ID

        public int PrestigeValue;
        public float IdleTime;             //闲置时间
    }



    public struct Family : IComponentData
    {
        public int FamilyId;
        public int ThisId;

    }


    public struct LivingArea : IComponentData
    {
        public int Id;
        public int ModelId;
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

        public Vector3 Position;
    }

    public struct Building : IComponentData
    {
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
        public Vector3 Position;
        public int Id;

        public LocationType InteractionType;
        public LocationType InteractionEnterType;
        public LocationType InteractionExitType;
        public int Distance;
        public ElementType Type;
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

}

