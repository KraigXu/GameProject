using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.UI;

public class TestRelationMapWindow : MonoBehaviour
{

    public RectTransform NodeParent;
    public RectTransform NodePrefab;
    public RectTransform LineParent;
    public RectTransform LinePrefab;
    public Dictionary<int, UIRelationNode> RelationNodeMap = new Dictionary<int, UIRelationNode>();
    public List<RectTransform> Lines=new List<RectTransform>();

    public RectTransform View;
    public Text InfoNameTxt;
    

    void Start()
    {

        List<RealtionRunData> realtionRunDatas = new List<RealtionRunData>();

        realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 3, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 4, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 5, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 6, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 7, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 8, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(1, 9, 10, 10, RealtionType.Friend));

        realtionRunDatas.Add(new RealtionRunData(2, 3, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(2, 4, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(2, 5, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(2, 6, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(2, 7, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(2, 8, 10, 10, RealtionType.Friend));
        realtionRunDatas.Add(new RealtionRunData(2, 9, 10, 10, RealtionType.Friend));

        realtionRunDatas.Add(new RealtionRunData(3, 4, 100, 89, RealtionType.Companion));

        realtionRunDatas.Add(new RealtionRunData(5, 3, 10, 10, RealtionType.Parents));

        realtionRunDatas.Add(new RealtionRunData(5, 4, 10, 10, RealtionType.Parents));

        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));
        //realtionRunDatas.Add(new RealtionRunData(1, 2, 10, 10, RealtionType.Friend));


        foreach (var item in realtionRunDatas)
        {
            UIRelationNode aNode;
            if (RelationNodeMap.ContainsKey(item.ida) == false)
            {
                aNode = UGUITools.AddChild(NodeParent.gameObject, NodePrefab.gameObject).GetComponent<UIRelationNode>();
                aNode.Entity = item.AEntity;
                aNode.Id = item.ida;
                aNode.callback = OnClickCallback;
                RelationNodeMap.Add(item.ida,aNode);
            }
            else
            {
                aNode = RelationNodeMap[item.ida];
            }

            UIRelationNode bNode;
            if (RelationNodeMap.ContainsKey(item.idb) == false)
            {
                bNode = UGUITools.AddChild(NodeParent.gameObject, NodePrefab.gameObject).GetComponent<UIRelationNode>();
                bNode.Entity = item.BEntity;
                bNode.Id = item.idb;
                bNode.callback = OnClickCallback;
                RelationNodeMap.Add(item.idb, bNode);
            }
            else
            {
                bNode = RelationNodeMap[item.idb];
            }


            aNode.AssociationNodes.Add(bNode);
            bNode.AssociationNodes.Add(aNode);

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCallback(UIRelationNode node)
    {
        node.NodeRect.anchoredPosition = Vector2.zero;

        List<UIRelationNode> uiNodes = node.AssociationNodes;

        float offset;
        float r = 40;
        int  ai = 0;
        float a0 = 30;

        float r2 = 200;
        float bi = 0;
        float b0 = 60;
        int childCount = LineParent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(LineParent.GetChild(i).gameObject);
        }

        foreach (var item in RelationNodeMap)
        {
            if (item.Key==node.Id)
            {
                item.Value.NodeRect.anchoredPosition = Vector2.zero;
            }else if (node.AssociationNodes.Contains(item.Value))
            {
                Vector2 pos;
                a0 = ai * 30;
                pos.x = 0 + r * Mathf.Cos(a0 * 3.14f / 180);
                Debug.Log(Mathf.Cos(a0 * 3.14f / 180));
                pos.y = 0 + r * Mathf.Sin(a0 * 3.14f / 180);
                item.Value.NodeRect.anchoredPosition = pos;
                ai++;

                RectTransform linenode= UGUITools.AddChild(LineParent.gameObject, LinePrefab.gameObject).transform as RectTransform;
                Vector2 off = pos - node.NodeRect.anchoredPosition;

                linenode.anchoredPosition= off / 2f;
                linenode.rotation=Quaternion.Euler(0,0,a0+90);
                linenode.sizeDelta=new Vector2(3,Vector2.Distance(pos,node.NodeRect.anchoredPosition));
            }
            else
            {
                Vector2 pos;
                b0 = bi * 30;
                pos.x = 0 + r2 * Mathf.Cos(b0 * 3.14f / 180);
                pos.y = 0 + r2 * Mathf.Sin(b0 * 3.14f / 180);
                item.Value.NodeRect.anchoredPosition = pos;
                bi++;
            }
        }
        
    }


    public void ShowInfo(UIRelationNode uiRelationNode)
    {
        View.gameObject.SetActive(true);
        InfoNameTxt.text = "";
        


    }

    public void CloseInfo(UIRelationNode uiRelation)
    {
        View.gameObject.SetActive(false);
    }


    

}
