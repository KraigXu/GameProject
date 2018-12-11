
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using GameSystem;

namespace GameSystem.Ui
{

    public class ShowWindowData
    {
        // Reset window
        public bool forceResetWindow = false;
        // force clear the navigation data
        public bool forceClearBackSeqData = false;
        // Object (pass data to target showed window)
        public BaseWindowContextData contextData;
        // Execute the navigation logic
        public bool executeNavLogic = true;
        // Check navigation 
        public bool checkNavigation = false;
        // force ignore add nav data
        public bool ignoreAddNavData = false;
    }

    // Base window data context for Refresh window or show window
    public class BaseWindowContextData { }


    /// <summary>
    /// RestWindowData
    /// </summary>
    public class RestWindowInData : BaseWindowContextData
    {
        public UnityAction OnExit;
        public UnityAction OnOpen;

        public UnityAction OnWuXueOpen;

        public List<Biological> Biologicals;
        
             
        
        public RestWindowInData() { }

        public RestWindowInData(UnityAction onExit,UnityAction onOpen)
        {
            this.OnExit = onExit;
            this.OnOpen = onOpen;

        }
    }

    




    public class StrategyWindowInData : BaseWindowContextData
    {

        public int PlayerAvatarId;
        public int PlayerId;

        public StrategyWindowInData(int id1,int  playerId)
        {
            this.PlayerAvatarId = id1;
            this.PlayerId = playerId;
        }
    }

    public class ExtendedMenuWindowInData : BaseWindowContextData
    {
        public UnityAction LivingAreEvent;
        public UnityAction DistrictEvent;


        public Vector3 Point;
        public int Id;
        public ExtendedMenuWindowInData(UnityAction livingAreEvent, UnityAction districtEvent, Vector3 point, int id)
        {
            this.LivingAreEvent = livingAreEvent;
            this.DistrictEvent = districtEvent;
            this.Point = point;
            this.Id = id;
        }

    }
    public class BiologicalUiInData : BaseWindowContextData
    {
        public int BiologicalId;
        public int PrestigeId;  //声望
        public int FactionId;  //势力
       

        public int Sex;

        public int Id;
        
        public int Prestige;
        public int Influence;
        public int Disposition;

        public int Age;
        public int AgeMax;

        public int Tizhi;
        public int Lidao;
        public int Jingshen;
        public int Lingdong;
        public int Wuxing;

        public int Jing;
        public int Qi;
        public int Shen;

        public BiologicalUiInData() { }


    }

    public delegate void EntityCallBack(Entity entity, int id);

    public class LivingAreaWindowCD : BaseWindowContextData
    {

        public int LivingAreaId;
        public int ModelId;
        public int PowerId;
        public int PersonId;
        public int PersonNumber;
        public int Money;
        public int MoneyMax;
        public int Iron;
        public int IronMax;
        public int Wood;
        public int WoodMax;
        public int Food;
        public int FoodMax;
        public int LivingAreaLevel;
        public int LivingAreaMaxLevel;
        public int LivingAreaType;
        public int DefenseStrength;
        public int StableValue;


        public EntityCallBack OnOpen;
        public EntityCallBack OnExit;
        public List<BuildingiDataItem> BuildingiDataItems = new List<BuildingiDataItem>();

        public LivingAreaWindowCD() { }
    }

    public class BuildingiDataItem
    {
        public int Id;
        public Entity OnlyEntity;
        public int Level;
        public int Status;
        public int ImageId;
        public Vector3 Point;

        public BuildingEvent OnOpen;
        public BuildingEvent OnClose;

        public List<BiologicalUiInfo> Biologicals= new List<BiologicalUiInfo>();
        public List<BuildingFeaturesUiInfo> Features=new List<BuildingFeaturesUiInfo>();
       
    }
    public class BiologicalUiInfo
    {
        public int Id;
        public int AtlasId;
        public string Name;
    }

    public class BuildingFeaturesUiInfo
    {
        public int Id;
        
    }




    public class WindowContextLivingAreaData : BaseWindowContextData
    {
        public List<int> EntityArray = new List<int>();
        public List<Vector3> Points = new List<Vector3>();
    }
    public delegate int[] SocialDialogEvent(int resoult,int a,int b);
    public class SocialDialogWindowData : BaseWindowContextData
    {
        public int Aid;
        public int Bid;

        public int PangBaiId;
        public int StartId;
        public int[] StartlogId;
        public int Relation;
        public SocialDialogEvent DialogEvent;
    }

    /// <summary>
    /// Ui界面上 BiologicalList
    /// </summary>
    public struct BiologicalUi
    {
        public int Id;   //唯一标识 id
        public int AvatarId;
        public int ModelId;

        public int SexId;
        public int Age;
        public int Disposition;

        public int PrestigeId;
        public int RelationId;
        public int FamilyId;

    }


    public struct LivingAreaUi
    {

    }




}