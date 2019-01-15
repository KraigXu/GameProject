using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景测试
/// </summary>
public class DemoTest : MonoBehaviour
{

    public string name3;
    public string name1;
    public string name2;

    public int Type;

    public int Names;
    public GameObject prefab;
    public int Rownumber=10;
    public int Columnumber=10;


    void Start()
    {

        //for (int i = 0; i <=Rownumber; i++)
        //{
        //    for (int j = 0; j <= Columnumber; j++)
        //    {
        //        GameObject go = GameObject.Instantiate(prefab);
        //        go.transform.position=new Vector3(i,0,j);
        //    }
        //}



    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 80, 30), "测试1"))
        {

        }

        if (GUI.Button(new Rect(0, 35, 80, 30), "测试2"))
        {

        }

    }

}
