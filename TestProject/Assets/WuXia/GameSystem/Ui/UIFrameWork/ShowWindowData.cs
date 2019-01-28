﻿
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
        public Biological CurPlayer;
        public List<Biological> Biologicals=new List<Biological>();
    }




    public class LivingAreaWindowCD : BaseWindowContextData
    {
        public int LivingAreaId;
        public int PlayerId;

        //public int PowerId;


        //public int ModelId;

       
        //public int PersonId;
        //public int PersonNumber;
        //public int Money;
        //public int MoneyMax;
        //public int Iron;
        //public int IronMax;
        //public int Wood;
        //public int WoodMax;
        //public int Food;
        //public int FoodMax;
        //public int LivingAreaLevel;
        //public int LivingAreaMaxLevel;
        //public int LivingAreaType;
        //public int DefenseStrength;
        //public int StableValue;


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
    public class BiologicalUiInfo : BaseWindowContextData
    {
        public int Id;
        public int AtlasId;
        public string Name;

        public int BiologicalId;
        public LocationType LocationType;
        public int LoactionId;
        public int TargetId;
        public Entity TargetEntity;
    }

    public class BuildingFeaturesUiInfo
    {
        public int Id;
    }

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

    public class FixedTitleWindowData : BaseWindowContextData
    {
        public List<KeyValuePair<string, Vector3>> Items;
    }

    

    /// <summary>
    /// 鼠标tips界面
    /// </summary>
    public class TipsInfoWindowData : BaseWindowContextData
    {
        public Vector3 Point;
        public bool IsShow;
        public int Id;
        public List<TipsInfoItemData> InfoItemDatas=new List<TipsInfoItemData>();
    }



    public class TipsInfoItemData
    {
        public string Title;
        public string Content;

        public TipsInfoItemData() { }

        public TipsInfoItemData(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }
    }

}