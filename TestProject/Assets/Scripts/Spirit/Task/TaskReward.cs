using UnityEngine;
using System.Collections;

public class TaskReward
{
    public string id;//奖励id
    public int amount = 0;//奖励数量

    public TaskReward(string id, int amount)
    {
        this.id = id;
        this.amount = amount;
    }
}
