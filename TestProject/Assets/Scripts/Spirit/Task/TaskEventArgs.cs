using UnityEngine;
using System.Collections;
using System;

public class TaskEventArgs : EventArgs {

    public string taskID;//当前任务的ID
    public string id;//发生事件的对象的ID(例如敌人,商品)
    public int amount;//数量
}
