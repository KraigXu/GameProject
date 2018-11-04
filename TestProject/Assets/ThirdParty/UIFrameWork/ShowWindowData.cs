using UnityEngine;
using System.Collections.Generic;

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
    public Strategy.LivingArea Node;

    public WindowContextLivingAreaNodeData(Strategy.LivingArea node)
    {
        this.Node = node;
    }
}


public class WindowContextExtendedMenu : BaseWindowContextData
{
    public Strategy.LivingArea LivingAreaNodeCom;
    public Vector3 Point;

    public WindowContextExtendedMenu(Strategy.LivingArea node, Vector3 point)
    {
        this.LivingAreaNodeCom = node;
        this.Point = point;
    }
}