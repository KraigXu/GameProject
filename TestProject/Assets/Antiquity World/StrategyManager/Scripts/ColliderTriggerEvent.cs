using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerEvent : MonoBehaviour
{
    public GameObjectCollider TriggerEnter;

    public GameObjectCollider TriggerExit;
    private Collider _collider;

    void Start()
    {
        _collider = gameObject.GetComponent<Collider>();
        if (_collider == null)
            _collider = gameObject.AddComponent<BoxCollider>();

    }

    void OnTriggerEnter(Collider other)
    {
       // Debug.Log(">>>1"+gameObject.name+"<<<<"+other.name);
        if (TriggerEnter != null)
        {

            TriggerEnter(gameObject, other);
        }


    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(">>>2");
        if (TriggerExit != null)
        {
            TriggerExit(gameObject, other);
        }
    }

}
