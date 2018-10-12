using System.Collections;
using System.Collections.Generic;
using MapMagicDemo;
using TinyFrameWork;
using UnityEngine;

public class Demo1 : MonoBehaviour {

    public CharController charController;
    public CameraController cameraController;
    public FlybyController demoController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetMouseButtonDown(1))
	    {

            charController.enabled = true;
            charController.gravity = false;
            charController.speed = 50;
            charController.acceleration = 150;
            demoController.enabled = false;
            cameraController.follow = 0;

	    }

	}
}
