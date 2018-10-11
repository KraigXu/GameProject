using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LivingArea;


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



