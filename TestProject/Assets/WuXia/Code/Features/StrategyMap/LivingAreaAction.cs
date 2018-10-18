using System.Collections;
using System.Collections.Generic;
using LivingArea;
using UnityEngine;


public abstract class LivingAreaAction
{
    public bool IsDisable;
    public abstract void Act(LivingAreaNode node);
}
public class LivingAreaAdd : LivingAreaAction
{
    public int value;
    public override void Act(LivingAreaNode node)
    {
        node.Renown += value;
    }
}

