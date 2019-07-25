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
        if (TriggerEnter != null)
        {

            TriggerEnter(gameObject, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (TriggerExit != null)
        {
            TriggerExit(gameObject, other);
        }
    }

}
