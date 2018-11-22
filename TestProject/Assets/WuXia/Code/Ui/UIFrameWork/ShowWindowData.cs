
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

    public class StrategyWindowInData : BaseWindowContextData
    {
        public UnityAction CharavterEvent;
        public UnityAction WugongEvent;
        public UnityAction TechnologyEvent;
        public UnityAction LogEvent;
        public UnityAction MapEvent;

        public StrategyWindowInData(UnityAction charavterEvent, UnityAction wugongEvent, UnityAction technologyEvent, UnityAction logEvent,
            UnityAction mapEvent)
        {
            this.CharavterEvent = charavterEvent;
            this.WugongEvent = wugongEvent;
            this.TechnologyEvent = technologyEvent;
            this.LogEvent = logEvent;
            this.MapEvent = mapEvent;
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

        public Entity OnlyEntity;
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

    public class LivingAreaWindowCD : BaseWindowContextData
    {

        public Entity OnlyEntity;
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
    }



    public class WindowContextLivingAreaData : BaseWindowContextData
    {
        public List<Entity> EntityArray = new List<Entity>();
        public List<Vector3> Points = new List<Vector3>();


    }

}