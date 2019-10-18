
using System;
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
        // Execute the navigation logic
        public bool executeNavLogic = true;
        // Check navigation 
        public bool checkNavigation = false;
        // force ignore add nav data
        public bool ignoreAddNavData = false;
        // Object (pass data to target showed window)
        public BaseWindowContextData contextData;
    }

    // Base window data context for Refresh window or show window
    public class BaseWindowContextData { }

    public class EntityContentData : BaseWindowContextData
    {
        public Entity Entity;
    }



    public class MessageBoxWindowData:BaseWindowContextData
    {
        public int Type;
        public string Content;
        public Action ConfirmAction;
        public Action CancelAction;
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

    public class WindowDataRelationMap : BaseWindowContextData
    {
        public List<RealtionRunData> Datas;

    }


    public class BiologicalUiInData : BaseWindowContextData
    {
        public Biological CurPlayer;
        public List<Biological> Biologicals = new List<Biological>();
        public List<Entity> Entities;
    }

    public class LivingAreaWindowCD : BaseWindowContextData
    {
        public Entity LivingAreaEntity;
        public int LivingAreaId;
        public int PlayerId;
        public EntityCallBack OnOpen;
        public EntityCallBack OnExit;
        public List<BuildingiDataItem> BuildingiDataItems = new List<BuildingiDataItem>();

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

    }


    public class BuildingUiInfo : BaseWindowContextData
    {
        public BuildingBlacksmithFeatures[] FeaturesUiInfos;
        public List<BiologicalUiInfo> Biologicals = new List<BiologicalUiInfo>();
    }

    public class BuildingBlacksmithFeatures
    {
        public int Id;
        public string Name;
        public string Type;
        public EntityCallBack CallBack;
    }

    public class DialogNode
    {
        public int Id;
        public int ParentId;
        public string Content;
        public int EventCode;

        public IList<DialogNode> Child = new List<DialogNode>();


        public void AddChild(DialogNode dialogNode)
        {
            this.Child.Add(dialogNode);
        }
    }

    public class SocialDialogWindowData : BaseWindowContextData
    {

        public int SceneId;
        public string Aside;
        public List<DialogNode> OnSelf = new List<DialogNode>();
        public List<DialogNode> Other = new List<DialogNode>();

        public Entity OnSelfEntity;
        public Entity OtherEntity;
    }

    public class FixedTitleWindowData : BaseWindowContextData
    {
        public List<KeyValuePair<string, Vector3>> Items;
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


    /// <summary>
    /// 鼠标tips界面
    /// </summary>
    public class TipsInfoWindowData : BaseWindowContextData
    {
        public Vector3 Point;
        public bool IsShow;
        public int Id;
        public List<TipsInfoItemData> InfoItemDatas = new List<TipsInfoItemData>();
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