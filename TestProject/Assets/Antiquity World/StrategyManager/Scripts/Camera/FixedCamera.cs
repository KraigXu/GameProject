using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Transform Target;
    public Camera _Camera;

    public Vector3 Offset;

	// Use this for initialization
	void Start () {
		
       transform.LookAt(Target);


	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = Target.position + Offset;

	    transform.LookAt(Target);
	}
}
