
using UnityEngine;
using UnityEngine.Events;

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
public class BaseWindowContextData{}

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

    public string BiologicalName;
    public string Race;
    public string Sex;
    public string Prestige;
    public string Influence;
    public string Disposition;

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

public class LivingAreaWindowCD:BaseWindowContextData
{
    public string LivingAreaName;

    public int HumanNumber;
    public int Id;
    public string Name;
    public string Description;
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
    public string BuildingInfoJson;
    public int PositionX;
    public int PositionY;
    public int PositionZ;

    public LivingAreaWindowCD() { }

    public LivingAreaWindowCD(string name)
    {
        this.LivingAreaName = name;
    }

}



public class WindowContextLivingAreaData : BaseWindowContextData
{
    public string[] Names;
    public Vector3[] Points;
    public WindowContextLivingAreaData(string[] names, Vector3[] points)
    {
        this.Names = names;
        this.Points = points;
    }
}

