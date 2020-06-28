using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AntiquityWorld.StrategyManager;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{

    public struct StatusInfo : IComponentData
    {
        public Vector3 Position;
        public Vector3 Face;
    }

    //---------------------------LivingArea




    /// <summary>
    /// 悬浮信息
    /// </summary>
    public struct FloatingInfo : IComponentData
    {
    }

    /// <summary>
    /// 行为数据
    /// </summary>
    public struct BehaviorData : IComponentData
    {
        public Vector3 Target;
        public int TargetId;
        public float TimeToLive;
        public Entity TargetEntity;
        public int CreantePositionCode;

        public Vector3 SelfPoint;
        public Vector3 NextPoint;
        public uint BehaviourType;

        public HexCoordinates Target1;
    }


    /// <summary>
    /// 关联数据
    /// </summary>
    public struct AssociationPropertyData : IComponentData
    {
        public int IsEntityOver;       //为1表示实体正常
        public int IsGameObjectOver;   //为1表示游戏物体关联

        public int ModelUid;         //模型编号
        public int IsModelShow;
        public Vector3 Position;
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

        public int RandomSeed;

        public float BehaviorTime;
    }

    public struct PlayerInput : IComponentData
    {
        public Vector3 MousePoint;
        public Vector2 MousePosition;
        public Vector3 ClickPoint;
        public Vector2 ViewMove;
        public Entity MouseEntity;
    }

    /// <summary>
    /// 玩家成员
    /// </summary>
    public struct PlayerMember : IComponentData
    {
        public int Id;

    }

    public struct District : IComponentData
    {
        public int Id;
        public int GId;
        public int Type;
        public int ProsperityLevel;
        public int TrafficLevel;
        public int GrowingModulus;
        public int SecurityModulus;
        public int Value;

        public int DistrictCode;

    }

    public struct Identity : IComponentData
    {

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

    /// <summary>
    /// 生物属性，其中参数大多是提升入手物体效率
    /// </summary>
    [Serializable]
    public struct Biological : IComponentData
    {
        public int BiologicalId;
        public int Age;
        public int Sex;
        public int CharmValue;
        public float Mobility;
        public float OperationalAbility;
        public float LogicalThinking;

        public int Jing;
        public int Qi;
        public int Shen;

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;

        public byte Thought;                                                  //思想
        public byte Neck;                                                     //脖子
        public byte Heart;                                                    //心脏
        public byte Eye;                                                      //眼睛
        public byte Ear;                                                      //耳朵
        public byte LeftLeg;                                                  //左腿
        public byte RightLeg;                                                 //右腿
        public byte LeftHand;                                                 //左手 
        public byte RightHand;                                                //右手
        public byte Fertility;                                                //生育
        public byte Skin;                                                     //皮肤
        public byte Blod;                                                     //血液
        public byte JingLuo;                                                  //经络

        public int AvatarId;
        public int ModelId;
        public int FamilyId;
        public int FactionId;
        public int TitleId;
        public int TechniquesId;
        public int EquipmentId;

        public int StrategyMoveBasSpeed;
        public int StrategyMoveSpeed;

        public int FireMoveSpeed;

        public int VisionBaseRange;
        public int VisionRange;

        public Entity Feature1;
        public Entity Feature2;
        public Entity Feature3;
        public Entity Feature4;
        public Entity Feature5;
        public Entity Feature6;

    }


    public class BiologicalFixed
    {
        public string Sex;
        public string Surname;
        public string Name;
        public string Description;
        public Sprite Sprite;
    }

    public class TeamFixed
    {
        public int Id;

        public string Name;
        public Transform Transform;
        public List<Entity> Members = new List<Entity>();
    }

    /// <summary>
    /// 身体属性 ，参数大多是提升物品使用效率
    /// </summary>
    public struct BodyProperty : IComponentData
    {

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;
    }

    public struct Fighting : IComponentData
    {
        public int AttackMin;
        public int AttackMax;
        public int PhysicsDefence;
        public int MagicDefence;


        public ushort ExpEmptyHand;                                            //空手
        public ushort ExpLongSoldier;                                          //长兵
        public ushort ExpShortSoldier;                                         //短兵
        public ushort ExpJones;                                                //奇兵
        public ushort ExpHiddenWeapone;                                        //射术
        public ushort ExpMedicine;                                             //医学
        public ushort ExpArithmetic;                                           //算术
        public ushort ExpMusic;                                                //音律
        public ushort ExpWrite;                                                //书法
        public ushort ExpDrawing;                                              //绘画
        public ushort ExpExchange;                                             //交流
        public ushort ExpTaoism;                                               //道法
        public ushort ExpDharma;                                               //佛法
        public ushort ExpPranayama;                                            //心法



    }

    public struct ExternalProperty : IComponentData
    {

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;

        public int HelmetProperty1;
        public int HelmetProperty2;
        public int HelmetProperty3;

        public byte Thought;
        public byte Neck;
        public byte Heart;
        public byte Eye;
        public byte Ear;
        public byte LeftLeg;
        public byte RightLeg;
        public byte LeftHand;
        public byte RightHand;
        public byte Fertility;
        public byte Skin;
        public byte Blod;
        public byte JingLuo;

        public int AvatarId;
        public int ModelId;
        public int FamilyId;
        public int FactionId;
        public int TitleId;
        public int TechniquesId;
        public int EquipmentId;

        public int StrategyMoveBasSpeed;
        public int StrategyMoveSpeed;
        
        public int FireMoveSpeed;
        public int VisionBaseRange;
        public int VisionRange;

    }


    public struct PositionInfo : IComponentData
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }


    public struct FixedProperty : IComponentData
    {
    }

    public struct FightingProperty : IComponentData
    {
        public int Strength;
        public int Skill;
        public int Defense;
        public int Magic;
        public int Luck;
        public int Charm;
        public int Mobility;
        public int Speed;

        public ushort ExpEmptyHand;                                            //空手
        public ushort ExpLongSoldier;                                          //长兵
        public ushort ExpShortSoldier;                                         //短兵
        public ushort ExpJones;                                                //奇兵
        public ushort ExpHiddenWeapone;                                        //射术
        public ushort ExpMedicine;                                             //医学
        public ushort ExpArithmetic;                                           //算术
        public ushort ExpMusic;                                                //音律
        public ushort ExpWrite;                                                //书法
        public ushort ExpDrawing;                                              //绘画
        public ushort ExpExchange;                                             //交流
        public ushort ExpTaoism;                                               //道法
        public ushort ExpDharma;                                               //佛法
        public ushort ExpPranayama;                                            //心法

        public Entity Skill1;
        public Entity Skill2;
        public Entity Skill3;
        public Entity Skill4;
        public Entity Skill5;
        public Entity Skill6;
        public Entity Skill7;
        public Entity Skill8;
        public Entity Skill9;
    }

    public struct NormalProperty : IComponentData
    {

    }
    public struct Life : IComponentData
    {
        public float Value;
    }

    public struct Energy : IComponentData
    {
        public float Value1;
        public float Value2;
    }

    public struct BiologicalAvatar : IComponentData
    {
        public int Id;
    }

    public struct ModelComponent : IComponentData
    {
        public float Speed;
        public Vector3 Target;
        public int Id;
      
        public float MoveSpeed;
    }


    public struct Team : IComponentData
    {
        public int TeamBossId;
        public int Member;
        public int Status;
    }

    public enum TargetType { None, City, Field, Biological }

    [SerializeField]
    public struct Family : IComponentData
    {
        public Entity FamilyBoss;
    }

    public struct FamilyProperty : IComponentData
    {
        public Entity TargetEntity;
    }


    public struct Faction : IComponentData
    {

        //public NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);
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


        public int Disposition;                                              //性格值
        public int NeutralValue;                                             //中立值
        public int LuckValue;                                                //运气值

        public int PrestigeValue;

    }

    public struct FactionProperty : IComponentData
    {
        public Entity FactionEntity;
        public int Level;
        public int FactionEntityId;
        public int Id;
    }

    public struct Ziggurat : IComponentData
    {
        public float Time;


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

        public Entity HelmetE;
        public Entity ClothesE;
        public Entity BeltE;
        public Entity HandGuardE;
        public Entity PantsE;
        public Entity ShoesE;
        public Entity WeaponFirstE;
        public Entity WeaponSecondaryE;
    }


    public struct EquipmentHelmet : IComponentData
    {
        public int Property1;
        public int Property2;
        public int Property3;
        public int Property4;

    }
    public enum EquipType
    {
        Coat,
        Underwear,
        Helmets,
        Necklace,
    }


    public enum EquipLevel
    {
        General = 1
    }

    public enum EquipPart
    {
        All = 0,
        Head = 1,
        Neck = 2,
        Chest = 3,
        Ring = 4,
        Leg = 5,
    }

    public struct Belong : IComponentData
    {
        public Entity Entity;
    }
    public struct EquipmentCoat : IComponentData
    {
        public int SpriteId;
        public EquipType Type;
        public EquipLevel Level;
        public EquipPart Part;
        public byte BluntDefense;
        public byte SharpDefense;
        public byte Operational;

        public float Weight;
        public float Price;
        public byte Durable;

    }

    /// <summary>
    /// 背包
    /// </summary>

    public struct Knapsack : IComponentData
    {
        public int UpperLimit; //克  负载量

        public int KnapscakCode;

        public int CurUpper;


    }

    public struct EquipmentHead : IComponentData
    {

    }

    /// <summary>
    /// Techniques属性 ，标识这个实体具有Techniques特性
    /// </summary>
    public struct TechniquesProperty : IComponentData
    {
        /// <summary>
        /// 增加速率
        /// </summary>
        public float IncreaseRate;

        /// <summary>
        /// 降低速率
        /// </summary>
        public float LowerRate;


    }

    public struct Techniques : IComponentData
    {
        public Entity BiologicalTarget;

        public int Level;


        public int TechniquesValue;
        public int Effect;
        public int Value;

    }

    public struct Wuxue : IComponentData
    {
        public int Value;
    }

    public enum LivingAreaType
    {
        SingleRoom = 1,  //独房
        Sect = 2,       //门派
        Village = 3,    //村庄
        City = 4,       //城市
    }

    public class BuildingItem
    {
        public int Id;
        public int Level;
        public string BuildingName = "XX";
        public string Behavior = "Default";
        public int X;
        public int Y;
        public int BuildingModelId;
        public int BuildingLevel;
        public int Status;
        public int Type;
        public int DurableValue;
        public string Property1;
        public string Property2;
        public string Property3;
        public string Property4;

        public BuildingItem()
        {
        }

        public BuildingItem(int buildingModelId, int buildingLevel, int status, int type, int durableValue)
        {
            this.BuildingModelId = buildingModelId;
            this.BuildingLevel = buildingLevel;
            this.Status = status;
            this.Type = type;
            this.DurableValue = durableValue;
        }
    }


    public class BuildingJsonData
    {
        public int GroupId;
        public List<BuildingItem> Item = new List<BuildingItem>();

        public BuildingItem GetBuildingItem(int buildingid)
        {
            for (int i = 0; i < Item.Count; i++)
            {
                if (Item[i].Id == buildingid)
                {
                    return Item[i];
                }
            }
            return null;
        }
    }

    public struct CellMap : IComponentData
    {
        public HexCoordinates Coordinates;

    }

    public struct LivingArea : IComponentData
    {
        public int Id;
        public byte IsInit;
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
        public byte TitleType;
    }

    public struct City : IComponentData
    {
        public int ModelId;
        public int UniqueCode;


        public int CityLevel;

        public int Type;
    }

    public struct CityMass : IComponentData
    {

    }

    public struct Crowd : IComponentData
    {
        public int Number;
        public int Money;
        public int Iron;

    }



    /// <summary>
    /// 集体，标明一个明确组织的生物群体 
    /// </summary>
    public struct Collective : IComponentData
    {
        public int Id;
        public int CollectiveClassId;
        public int Cohesion;
    }

    public struct ModelInfo : IComponentData
    {
        public int ModelId;
    }

    public struct Money : IComponentData
    {
        public int Value;
        public int Upperlimit;
    }

    public struct Reputation : IComponentData
    {

    }


    public struct LivingAreaAssociated : IComponentData
    {
        public Entity LivingAreaEntity;
        public Entity BuildingEntity;
    }
    /// <summary>
    /// 裁缝
    /// </summary>
    public struct BuildingTailor : IComponentData
    {

    }

    /// <summary>
    /// 客栈
    /// </summary>
    public struct BuidingTavern : IComponentData
    {

    }





    /// <summary>
    /// 建筑物：铁匠铺
    /// </summary>
    public struct BuildingBlacksmith : IComponentData
    {

        public Entity Person;
        public int LevelId;
        public int OperateStart;
        public int OperateEnd;
        public int ShopSeed;

        public int PositionCode;

    }

    /// <summary>
    /// 建筑物：市集
    /// </summary>
    public struct BuildingBazaar : IComponentData
    {
        public int LevelId;
        public int OperateStart;
        public int OperateEnd;
        public int ShopSeed;

        public int PositionCode;
    }

    /// <summary>
    /// 房屋控制权
    /// </summary>
    public struct HousesControl : IComponentData
    {
        public int SeedId;
        public Entity Entity1;
        public Entity Entity2;
        public Entity Entity3;
        public Entity Entity4;

        public int No1;
        public int No2;
        public int No3;
    }

    public struct BuildingFlock : IComponentData
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

        //        public int LoactionUnity;
    }

    public struct PrestigeValue : IComponentData
    {
        public int value;
    }

    public struct Position : IComponentData
    {
        public float x;
        public float y;
        public float z;

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

    public struct Relation : IComponentData
    {
        public int ObjectAid;
        public int ObjectBid;
        public int Value;

    }
    public enum RealtionType
    {
        Friend = 0,
        Parents = 1,
        Companion = 2,
        Enemy = 3,


    }


    public class RealtionRunData
    {
        public int ida;
        public int idb;

        public Entity AEntity;
        public byte AValue;
        public Entity BEntity;
        public byte BValue;
        public RealtionType Type;


        public RealtionRunData(Entity a, Entity b, byte avalue, byte bvalue, RealtionType type)
        {
            this.AEntity = a;
            this.BEntity = b;
            this.AValue = avalue;
            this.BValue = bvalue;
            this.Type = type;
        }
        public RealtionRunData(int a, int b, byte avalue, byte bvalue, RealtionType type)
        {
            this.ida = a;
            this.idb = b;
            this.AValue = avalue;
            this.BValue = bvalue;
            this.Type = type;
        }

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

    /// <summary>
    /// 道具
    /// </summary>
    public struct ArticleItem : IComponentData
    {

        public Entity BiologicalEntity;
        public int GuiId;
        public int Count;
        public int MaxCount;
        public int Weight;
        public int SpriteId;

        public ENUM_OBJECT_TYPE ObjectType;   //对象类型
        public ENUM_OBJECT_STATE ObjectState;  //对象状态

        public int BiologicalId;
        public ENUM_ITEM_CLASS Type;
        public ENUM_ITEM_ATTRIBUTE Attribute1;
        public int AttributeValue1;
        public ENUM_ITEM_ATTRIBUTE Attribute2;
        public int AttributeValue2;
        public ENUM_ITEM_ATTRIBUTE Attribute3;
        public int AttributeValue3;
        public ENUM_ITEM_ATTRIBUTE Attribute4;
        public int AttributeValue4;
        public ENUM_ITEM_ATTRIBUTE Attribute5;
        public int AttributeValue5;
        public ENUM_ITEM_ATTRIBUTE Attribute6;
        public int AttributeValue6;
        public ENUM_ITEM_ATTRIBUTE Attribute7;
        public int AttributeValue7;
        public ENUM_ITEM_ATTRIBUTE Attribute8;
        public int AttributeValue8;
    }


    /// <summary>
    /// ArticleItem的固有属性 
    /// </summary>
    public class ArticleItemFixed
    {

        public string Name;
        public string Desc;
        public Sprite Sprite;

        public string Expain;

    }


    public struct AttackProperty : IComponentData
    {
        public int Blunt;
        public int Sharp;
        public int Dexterous;
        public int Parry;

        public KeyValuePair<ENUM_ITEM_ATTRIBUTE, int> Attribute1;
        public KeyValuePair<ENUM_ITEM_ATTRIBUTE, int> Attribute2;
        public KeyValuePair<ENUM_ITEM_ATTRIBUTE, int> Attribute3;
        public KeyValuePair<ENUM_ITEM_ATTRIBUTE, int> Attribute4;
        public KeyValuePair<ENUM_ITEM_ATTRIBUTE, int> Attribute5;
        public KeyValuePair<ENUM_ITEM_ATTRIBUTE, int> Attribute6;

    }



    public struct DefenseProperty : IComponentData
    {
        public int BaseValue;
        public int Blunt;
        public int Sharp;

        public ENUM_ITEM_ATTRIBUTE Attribute1;
        public ENUM_ITEM_ATTRIBUTE Attribute2;
        public ENUM_ITEM_ATTRIBUTE Attribute3;
        public ENUM_ITEM_ATTRIBUTE Attribute4;

    }

    public struct Speciality : IComponentData
    {

    }

    //public struct TeamColor:IComponentData,IInstanceRenderProperties 
    //{public float4 Color;
    //} 
    //  struct TeamColor : IComponentData, IInstanceRenderProperties { public float4 Color; }


    public struct Camp : IComponentData
    {
        public int UniqueCode;
    }

    public struct Timer : IComponentData
    {
        // public int TimeType;   //1地形 //2生物 
        // public float TimeAdd;


        public int ExpendDay;
        public int DayEnd;

    }

    public struct RotationSpeed : IComponentData
    {

    }

}

