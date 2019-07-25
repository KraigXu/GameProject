using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMenuFeatureButton : MonoBehaviour
{

    public ActionDefault Action;
    [SerializeField]
    private Button _button;



    void Awake()
    {
      //  _button.onClick.AddListener(Action);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
