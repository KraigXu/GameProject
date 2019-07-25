using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskPanel : MonoSingletion<TaskPanel> {

    public Dictionary<string, TaskItem> dictionary = new Dictionary<string, TaskItem>();//id,taskItem

    public GameObject content;//内容
    public Vector2 contentSize;//内容的原始高度

    public GameObject item;//列表项
    public float itemHeight;
    public Vector3 itemLocalPos;
    
    void Start()
    {
        content = transform.Find("Image/Content").gameObject;
        contentSize = content.GetComponent<RectTransform>().sizeDelta;

        item = Resources.Load("Item") as GameObject;
        itemHeight = item.GetComponent<RectTransform>().rect.height;
        itemLocalPos = item.transform.localPosition;

        TaskManager.Instance.getEvent += AddItem;
        TaskManager.Instance.rewardEvent += RemoveItem;
        TaskManager.Instance.cancelEvent += RemoveItem;
    }

    //添加列表项
    public void AddItem(System.Object sender, TaskEventArgs e)
    {
        GameObject a = Instantiate(item) as GameObject;
        a.transform.parent = content.transform;
        a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - dictionary.Count * itemHeight, 0);

        TaskItem t = a.GetComponent<TaskItem>();
        dictionary.Add(e.taskID,t);
        t.Init(e);

        if (contentSize.y <= dictionary.Count * itemHeight)//增加内容的高度
        {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, dictionary.Count * itemHeight);
        }
    }

    //移除列表项
    public void RemoveItem(System.Object sender, TaskEventArgs e)
    {
        if (dictionary.ContainsKey(e.taskID))
        {
            TaskItem t = dictionary[e.taskID];
            dictionary.Remove(e.taskID);
            Destroy(t.gameObject);

            //调整位置
            int count = 0;
            foreach (KeyValuePair<string, TaskItem> kv in dictionary)
            {
                kv.Value.gameObject.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - count * itemHeight, 0);
                count++;
            }

            if (contentSize.y <= dictionary.Count * itemHeight)//调整内容的高度     
                content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, dictionary.Count * itemHeight);
            else
                content.GetComponent<RectTransform>().sizeDelta = contentSize;
        }      
    }
}
