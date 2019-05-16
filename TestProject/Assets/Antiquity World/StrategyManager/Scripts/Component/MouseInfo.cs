using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInfo : MonoBehaviour
{

    public static MouseInfo Instance
    {
        get { return _instance; }
    }
    private static MouseInfo _instance;




    void Awake()
    {
        _instance = this;
    }


	void Start () {

	}
	
	void Update () {
      //  Debug.Log(_eventSystem.currentSelectedGameObject.name);
	   
        

	}

    void OnGUI()
    {


    }
}
