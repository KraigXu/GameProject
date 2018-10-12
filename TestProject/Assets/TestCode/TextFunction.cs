using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LivingArea;
using BuildingFeatures = LivingArea.BuildingFeatures;


public class TextFunction : MonoBehaviour
{

    public Text log;
    public InputField ipt;

	// Use this for initialization
	void Start () {
		BuildingObject building =new BuildingObject("悦来客栈","这个是客栈说明",1,1,100,300,1,1,"1;2;3");         //新增一个建筑物

        //解析设施功能

	    string[] featuresIds = building.BuildingFeaturesIds.Split(';');


        BuildingFeatures buildingFeature = new BuildingFeatures();


        //for (int i = 0; i < featuresIds; i++)
        //{

        //}

	    string features = ""; 

        log.text = "设施名："+building.Name+"\n"+
            "等级："+building.BuildingLevel+"\n"+
            "说明："+building.Description+"\n"+
            "当前耐久："+building.DurableValue+"\n"+
            "最大耐久："+building.DurableMax+"\n"+
            "状态："+building.BuildingStatus+"\n"+
            "所属人："+building.HaveId+"\n"+
            "拥有功能："+building.BuildingFeaturesIds+"\n\n\n"+
            "请问你选择哪个功能：" + features ;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
