using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour {

    public Task task;//对应的任务逻辑

    public Text taskName;
    public Text caption;
    public Text buttonText;

    public GameObject process;
    public Vector3 processLocalPos;
    public List<TaskItemProcess> processText = new List<TaskItemProcess>();

    public GameObject reward;
    public Vector3 rewardLocalPos;
    public List<TaskItemReward> rewardText = new List<TaskItemReward>();

    public void Init(TaskEventArgs e)
    {      
        process = Resources.Load("Process") as GameObject;
        processLocalPos = process.transform.localPosition;
        reward = Resources.Load("Reward") as GameObject;
        rewardLocalPos = reward.transform.localPosition;

        task = TaskManager.Instance.dictionary[e.taskID];
        task.taskItem = this;

        taskName.text = task.taskName;
        caption.text = task.caption;

        for (int i = 0; i < task.taskConditions.Count; i++)
        {
            GameObject a = Instantiate(process) as GameObject;
            a.transform.parent = transform;
            a.transform.localPosition = new Vector3(processLocalPos.x, processLocalPos.y - processText.Count * 20, 0);

            TaskItemProcess tP = a.GetComponent<TaskItemProcess>();
            processText.Add(tP);

            tP.id.text = task.taskConditions[i].id;
            tP.now.text = task.taskConditions[i].nowAmount.ToString();
            tP.target.text = task.taskConditions[i].targetAmount.ToString();
        }

        for (int i = 0; i < task.taskRewards.Count; i++)
        {
            GameObject a = Instantiate(reward) as GameObject;
            a.transform.parent = transform;
            a.transform.localPosition = new Vector3(rewardLocalPos.x, rewardLocalPos.y - rewardText.Count * 20, 0);

            TaskItemReward tR = a.GetComponent<TaskItemReward>();
            rewardText.Add(tR);

            tR.id.text = task.taskRewards[i].id;
            tR.amount.text = task.taskRewards[i].amount.ToString();
        }
    }

    //修改条件的当前进度
    public void Modify(string id,int amount)
    {
        for (int i = 0; i < processText.Count; i++)
        {
            if (processText[i].id.text == id)
                processText[i].now.text = amount.ToString();
        }     
    }

    public void Finish(bool isFinish)
    {
        if (isFinish)
            buttonText.text = "完成了";
        else
            buttonText.text = "未完成";
    }

    public void Reward()
    {
        if (buttonText.text == "完成了")   
            task.Reward();
    }

    public void Cancel()
    {
        task.Cancel();
    }
}
