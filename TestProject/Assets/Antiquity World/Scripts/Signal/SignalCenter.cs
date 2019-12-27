
using System;
using Signals;
using UnityEngine;
public static class SignalCenter
{

    //Mouse
    public static MouseSignal MouseOnMove=new MouseSignal();
    public static MouseSignal MouseOnRight = new MouseSignal();
    public static MouseSignal MouseOnRightUp = new MouseSignal();
    public static MouseSignal MouseOnLeftUp = new MouseSignal();
    public static MouseSignal MouseOnLeftDown = new MouseSignal();
    public static MouseSignal MouseOnMiddle = new MouseSignal();
    public static MouseSignal MouseOnMiddleDown = new MouseSignal();
    public static MouseSignal MouseOnMiddleUp = new MouseSignal();
    public static MouseSignal MouseOnLeaveView = new MouseSignal();
    public static MouseSignal MouseOnEnterView = new MouseSignal();
    public static MouseSignal MouseOnScrollWheel = new MouseSignal();

    //Data

    public static DataLoad GameDataLoadOver =new DataLoad();

    //SystemData
    public static BiologicalEntityData BiologicalDataChange=new BiologicalEntityData();






}
