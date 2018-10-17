using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using DataAccessObject;
using LitJson;

public class Task {

    public TaskItem taskItem;//对应的UI面板

    public string taskID;//任务ID
    public string taskName;//任务名字
    public string caption;//任务描述   
    public List<TaskCondition> taskConditions = new List<TaskCondition>();
    public List<TaskReward> taskRewards = new List<TaskReward>();

    //根据taskNum读取xml,实现初始化
    public Task(string taskID)
    {
        this.taskID = taskID;
        XElement xe = TaskManager.Instance.rootElement.Element(taskID);
        taskName = xe.Element("taskName").Value;
        caption = xe.Element("caption").Value;

        IEnumerable<XElement> a = xe.Elements("conditionID");
        IEnumerator<XElement> b = xe.Elements("conditionTargetAmount").GetEnumerator();

        IEnumerable<XElement> c = xe.Elements("rewardID");
        IEnumerator<XElement> d = xe.Elements("rewardAmount").GetEnumerator();

        foreach (var s in a)
        {
            b.MoveNext();
            TaskCondition tc = new TaskCondition(s.Value, 0, int.Parse(b.Current.Value));
            taskConditions.Add(tc);
        }

        foreach (var s in c)
        {
            d.MoveNext();
            TaskReward tr = new TaskReward(s.Value, int.Parse(d.Current.Value));
            taskRewards.Add(tr);
        }
    }

    public Task(int taskId)
    {
        this.taskID = taskID.ToString();
        TaskModel model=   SqlData.GetModelId<TaskModel>(taskId);
        taskName = model.TaskName;
        caption = model.Caption;
        taskConditions = JsonMapper.ToObject<List<TaskCondition>>(model.TaskConditionsJson);
        taskRewards = JsonMapper.ToObject<List<TaskReward>>(model.TaskRewardsJson);
    }

    //判断条件是否满足
    public void Check(TaskEventArgs e)
    {
        TaskCondition tc;
        for (int i = 0; i < taskConditions.Count; i++)
        {
            tc = taskConditions[i];
            if (tc.id == e.id)
            {
                tc.nowAmount += e.amount;
                if(tc.nowAmount < 0) tc.nowAmount = 0;
                if (tc.nowAmount >= tc.targetAmount)
                    tc.isFinish = true;
                else
                    tc.isFinish = false;

                taskItem.Modify(e.id,tc.nowAmount);
            }
        }

        for (int i = 0; i < taskConditions.Count; i++)
        {
            if (!taskConditions[i].isFinish)
            {
                taskItem.Finish(false);
                return;
            }
        }
        taskItem.Finish(true);
        e.taskID = taskID;
        TaskManager.Instance.FinishTask(e);
    }

    //获取奖励
    public void Reward()
    {
        TaskEventArgs e = new TaskEventArgs();
        e.taskID = taskID;
        TaskManager.Instance.GetReward(e);
    }

    //取消任务
    public void Cancel()
    {
        TaskEventArgs e = new TaskEventArgs();
        e.taskID = taskID;
        TaskManager.Instance.CancelTask(e);
    }
}
