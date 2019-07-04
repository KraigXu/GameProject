using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{

    public RectTransform node;

	// Use this for initialization
	void Start () {
        //node.anchorMax=new Vector2(10,10);
		node.offsetMax=new Vector2(80,75);
	    node.offsetMin =new Vector2(0,-75);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
