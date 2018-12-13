using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDic : MonoBehaviour {

    public Dictionary<int,int> mRedpointDic=new Dictionary<int, int>();

    public List<int> mRedpointActId=new List<int>();

    public Dictionary<int, string> mRedpointDic1 = new Dictionary<int, string>();
    // Use this for initialization
    void Start () {

	    mRedpointDic.Add(1,1);
	    mRedpointDic.Add(2, 2);
	    mRedpointDic.Add(3, 3);
	    mRedpointDic.Add(4, 4);
	    //int resValue;
        //mRedpointDic1.TryGetValue(key, out resValue);
    }
	
	// Update is called once per frame
	void Update () {
        //mRedpointActId.Clear();

        //if (null != mRedpointDic)
        //{
        //    foreach (int redPointKey in mRedpointDic.Keys)
        //    {
        //        if (mRedpointDic[redPointKey] > 0)
        //        {
        //            if (!mRedpointActId.Contains(redPointKey))
        //            {
        //                mRedpointActId.Add(redPointKey);
        //            }
        //        }
        //    }
        //}

        //mRedpointActId.Clear();
        //if (null != mRedpointDic)
        //{
        //    var ge = mRedpointDic.GetEnumerator();
        //    while (ge.MoveNext())
        //    {
        //        if (ge.Current.Value > 0)
        //        {
        //            if (!mRedpointActId.Contains(ge.Current.Key))
        //            {
        //                mRedpointActId.Add(ge.Current.Key);
        //            }
        //        }
        //    }
        //    ge.Dispose();
        //}


    }
}
