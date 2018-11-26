using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WX
{
    public enum TragetType { Idie, City, Field }

    public struct CameraProperty : IComponentData
    {
        public Vector3 Target;
        public int Damping;
        public Vector3 Offset;
        public Vector3 RoationOffset;
    }
    /// <summary>
    /// 可交互物
    /// </summary>
    public struct Interactable : IComponentData
    {
        public int InteractionType;
    }

    public enum TendType { Money,Move }

    /// <summary>
    /// Npc
    /// </summary>
    public struct NpcInput : IComponentData
    {
        //趋向
        public int Movetend;

    }
    public struct PlayerInput : IComponentData{}
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

        public int TragetId;
        
        public Vector3 CurTarget;
    }

    public struct Family : IComponentData
    {
        public int FamilyId;
        public int ThisId;

    }

    public struct BiologicalStatus: IComponentData
    {
        public Vector3 Position;        //  位置
        public int TargetId;            // 目标ID
        public int TargetType;          // 目标类型
        public int StatusRealTime;       // 实时状态
        public int LocationId;           //所处位置ID

        public int PrestigeValue;

    }

    public struct InteractionElement : IComponentData
    {
        public Vector3 Position;
        public int Id;
        public int InteractionType;
        public int InteractionEnterType;
        public int InteractionExitType;
        public int Distance;
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



}

