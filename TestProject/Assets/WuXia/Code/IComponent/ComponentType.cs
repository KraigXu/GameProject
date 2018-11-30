using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WX
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

    public enum TendType
    {
        None=0, //无
        Patrol=1,  //巡逻

    }

    /// <summary>
    /// Npc
    /// </summary>
    public struct NpcInput : IComponentData
    {
        //趋向
        public TendType Movetend;

        public int RandomSeed;

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

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;

        public int Jing;
        public float RestoreJing;
        public int CurJing;
        public int Qi;
        public float RestoreQi;
        public int CurQi;
        public int Shen;
        public float RestoreShen;
        public int CurShen;

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

        public int TragetId;
        
        public Vector3 CurTarget;
    }

    public struct Family : IComponentData
    {
        public int FamilyId;
        public int ThisId;

    }

    public enum TargetType {  None,City, Field, Biological }
    public struct BiologicalStatus: IComponentData
    {
        public Vector3 Position;        //  位置
        public int TargetId;            // 目标ID
        public ElementType TargetType;          // 目标类型
        public Vector3 TargetPosition;       //目标位置
        public LocationType LocationType;       // 实时状态
        public int LocationId;           //所处位置ID

        public int PrestigeValue;

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

