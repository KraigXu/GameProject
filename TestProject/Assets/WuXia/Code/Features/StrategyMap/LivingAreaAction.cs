using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LivingAreaAction
{
    public bool IsDisable;
    public abstract void Act(WX.LivingArea node);
}
public class LivingAreaAdd : LivingAreaAction
{
    public int value;
    public override void Act(WX.LivingArea node)
    {
        node.Renown += value;
    }
}

