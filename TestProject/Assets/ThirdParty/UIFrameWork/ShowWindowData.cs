
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
public class BaseWindowContextData
{
}

public class StrategyWindowInData:BaseWindowContextData
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

public class BiologicalUiInData:BaseWindowContextData
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


public class WindowContextLivingAreaData : BaseWindowContextData
{
    public string[] Names;
    public Vector3[] Points;
    public WindowContextLivingAreaData(string[] names,Vector3[] points)
    {
        this.Names = names;
        this.Points = points;
    }
}

public class WindowContextLivingAreaNodeData : BaseWindowContextData
{
    public WX.LivingArea Node;

    public WindowContextLivingAreaNodeData(WX.LivingArea node)
    {
        this.Node = node;
    }
}


public class WindowContextExtendedMenu : BaseWindowContextData
{
    public WX.LivingArea LivingAreaNodeCom;
    public Vector3 Point;

    public WindowContextExtendedMenu(WX.LivingArea node, Vector3 point)
    {
        this.LivingAreaNodeCom = node;
        this.Point = point;
    }
}