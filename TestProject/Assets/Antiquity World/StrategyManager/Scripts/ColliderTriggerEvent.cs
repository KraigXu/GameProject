using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerEvent : MonoBehaviour
{
    public GameObjectCollider TriggerEnter;

    public GameObjectCollider TriggerExit;
    


    private Collider _collider;



	void Start ()
	{
	    _collider = gameObject.GetComponent<Collider>();
	    if (_collider == null)
	        _collider= gameObject.AddComponent<BoxCollider>();


	}
	
    void OnCollisionEnter(Collision collision)
    {
        if (TriggerEnter != null)
        {
            
            TriggerEnter(gameObject,collision);
        }


    }

    void OnCollisionExit(Collision collision)
    {
        if (TriggerExit != null)
        {

            TriggerExit(gameObject, collision);
        }
    }

}
