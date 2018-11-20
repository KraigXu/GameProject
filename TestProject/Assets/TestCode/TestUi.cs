using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUi : MonoBehaviour
{

    public BaseCorrespondenceByModelControl ui;
    // Use this for initialization
    void Start () {
        UIEventTriggerListener.Get(ui.gameObject).onClick += SetUpdate;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetUpdate(GameObject go)
    {
        Debug.Log("<<<<,SS");
    }
}
