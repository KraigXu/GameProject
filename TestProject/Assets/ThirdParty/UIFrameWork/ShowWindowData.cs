using UnityEngine;
using System.Collections;
using LivingArea;
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
    public List<LivingAreaNode> Nodes;
    public WindowContextLivingAreaData(List<LivingAreaNode> nodes)
    {
        this.Nodes = nodes;
    }
}

public class WindowContextLivingAreaNodeData : BaseWindowContextData
{
    public LivingAreaNode Node;

    public WindowContextLivingAreaNodeData(LivingAreaNode node)
    {
        this.Node = node;
    }
}


public class WindowContextExtendedMenu : BaseWindowContextData
{
    public LivingAreaNode LivingAreaNodeCom;
    public Vector3 Point;

    public WindowContextExtendedMenu(LivingAreaNode node, Vector3 point)
    {
        this.LivingAreaNodeCom = node;
        this.Point = point;
    }
}