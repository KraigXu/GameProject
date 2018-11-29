﻿
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using WX;

namespace WX.Ui
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

    public class MenuEventData:BaseWindowContextData
    {

        public UnityAction Rest;

        public UnityAction Article;

        public UnityAction Team;

        public UnityAction Recording;

        public UnityAction Log;

        public UnityAction Relationship;

        public MenuEventData(UnityAction rest,UnityAction article,UnityAction team,UnityAction recording,UnityAction log,UnityAction relationship)
        {
            this.Rest = rest;
            this.Article = article;
            this.Team = team;
            this.Recording = recording;
            this.Log = log;
            this.Relationship = relationship;
        }
    }


    public class StrategyWindowInData : BaseWindowContextData
    {

        public int PlayerAvatarId;
        public int PlayerId;

        public UnityAction CharavterEvent;
        public UnityAction WugongEvent;
        public UnityAction TechnologyEvent;
        public UnityAction LogEvent;
        public UnityAction MapEvent;

        public StrategyWindowInData(UnityAction charavterEvent, UnityAction wugongEvent, UnityAction technologyEvent, UnityAction logEvent,
            UnityAction mapEvent,int id1,int  playerId)
        {
            this.CharavterEvent = charavterEvent;
            this.WugongEvent = wugongEvent;
            this.TechnologyEvent = technologyEvent;
            this.LogEvent = logEvent;
            this.MapEvent = mapEvent;
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
        public int Id;
        public int Sex;
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


        //public Dictionary<int, List<int>> BiologicalBranch=new Dictionary<int, List<int>>();
        //public 

        

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
       
    }

    /// <summary>
    /// 分支组
    /// </summary>
    public class BranchGroup
    {
        //public int Bio
        

    }

    public class BranchNode
    {
        public int AskKey;
        public int AnswerKey;
    }

    public class BiologicalUiInfo
    {
        public int Id;
        public int AtlasId;
        public string Name;
    }


    public class WindowContextLivingAreaData : BaseWindowContextData
    {
        public List<int> EntityArray = new List<int>();
        public List<Vector3> Points = new List<Vector3>();
    }
    public delegate int[] SocialDialogEvent(int id);
    public class SocialDialogWindowData : BaseWindowContextData
    {
        public int Aid;
        public int Bid;

        public int PangBaiId;
        public int StartId;
        public int[] StartlogId;
        public SocialDialogEvent DialogEvent;
    }

}