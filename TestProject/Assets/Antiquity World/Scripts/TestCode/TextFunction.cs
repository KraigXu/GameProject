using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using GameSystem;


public class TextFunction : MonoBehaviour
{

    public Text log;
    public InputField ipt;

	// Use this for initialization
	void Start () {
       // BuildingObject building1 =new BuildingObject("悦来客栈","这个是客栈说明",1,1,100,300,1,1,"1;2;3");         //新增一个建筑物
     //  BuildingObject[] buildingObjects=new BuildingObject[3];

     //   buildingObjects[0]=new BuildingObject("A101","悦来客栈", "这个是客栈说明",3,BuildingStatus.None, BuildingType.Rest,100,1,"1;2","1;2","");

	    //buildingObjects[1] = new BuildingObject("A102", "比武擂台", "这个是比武擂台说明", 3, BuildingStatus.None, BuildingType.Rest, 100, 1, "1;2", "1;2", "");

	    //buildingObjects[2] = new BuildingObject("A103", "民居", "这个是客栈说明", 3, BuildingStatus.None, BuildingType.Rest, 100, 1, "1;2", "1;2", "");

     //   Debug.Log(JsonConvert.SerializeObject(buildingObjects));
     //   ////解析设施功能

        //string[] featuresIds = building.BuildingFeaturesIds.Split(';');


        //BuildingFeatures buildingFeature = new BuildingFeatures();
        //buildingFeature.Name = "筵席";
        //buildingFeature.Description = "请场景内所有人喝酒，花费1000钱";

        //BuildingFeatures building1=new BuildingFeatures();
        //building1.Name = "修炼";
        //building1.Description = "话时间修炼";
        //BuildingFeatures building2 = new BuildingFeatures();
        //building2.Name = "研习";
        //building2.Description = "花时间修炼";


        ////for (int i = 0; i < featuresIds; i++)
        ////{

        ////}

        //string features = "1："+buildingFeature.Name+"  2："+ building1.Name+"  3："+ building2.Name; 

        //log.text = "设施名："+building.Name+"\n"+
        //    "等级："+building.BuildingLevel+"\n"+
        //    "说明："+building.Description+"\n"+
        //    "当前耐久："+building.DurableValue+"\n"+
        //    "最大耐久："+building.DurableMax+"\n"+
        //    "状态："+building.BuildingStatus+"\n"+
        //    "所属人："+building.HaveId+"\n"+
        //    "拥有功能："+building.BuildingFeaturesIds+"\n\n\n"+
        //    "请问你选择哪个功能：" + features ;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
