using System.Collections;
using System.Collections.Generic;
using LivingArea;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示居住地标题
/// </summary>
public class LivingAreaTitleItem : BaseCorrespondenceByModelControl
{

    [SerializeField]
    private Text _titleText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Init(Transform  target)
    {
        this.Target = target;

        LivingAreaNode node=  target.GetComponent<LivingAreaNode>();
        _titleText.text = node.Name;

    }
}
