using System.Collections;
using System;

public class TaskCondition
{
    public string id;//条件id
    public int nowAmount;//条件id的当前进度
    public int targetAmount;//条件id的目标进度
    public bool isFinish = false;//记录是否满足条件

    public TaskCondition(string id, int nowAmount, int targetAmount)
    {
        this.id = id;
        this.nowAmount = nowAmount;
        this.targetAmount = targetAmount;
    }
}
