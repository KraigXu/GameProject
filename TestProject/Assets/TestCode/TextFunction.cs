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
	    buildingFeature.Name = "筵席";
	    buildingFeature.Description = "请场景内所有人喝酒，花费1000钱";

	    BuildingFeatures building1=new BuildingFeatures();
	    building1.Name = "修炼";
	    building1.Description = "话时间修炼";
	    BuildingFeatures building2 = new BuildingFeatures();
	    building2.Name = "研习";
	    building2.Description = "花时间修炼";


        //for (int i = 0; i < featuresIds; i++)
        //{

        //}

        string features = "1："+buildingFeature.Name+"  2："+ building1.Name+"  3："+ building2.Name; 

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
