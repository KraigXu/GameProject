using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LivingAreaAction
{
    public bool IsDisable;
    public abstract void Act(GameSystem.LivingArea node);
}
public class LivingAreaAdd : LivingAreaAction
{
    public int value;
    public override void Act(GameSystem.LivingArea node)
    {
        //node.Renown += value;
    }
}

