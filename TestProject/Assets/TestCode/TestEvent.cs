using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestEvent : MonoBehaviour
{
    [SerializeField]
    private EventSystem eventSystem;
	// Use this for initialization
	void Start ()
	{
	    //eventSystem = transform.GetComponent<EventSystem>();
	    //eventSystem.a

    }
	
	// Update is called once per frame
	void Update () {
	    if (eventSystem.currentSelectedGameObject != null)
	    {
	        Debug.Log(eventSystem.currentSelectedGameObject.name);
        }
		
	}
}
