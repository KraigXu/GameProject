using WX;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Newtonsoft.Json;
using TinyFrameWork;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace WX
{
    /// <summary>
    /// LivingArea：居住地类型 影响本身的逻辑
    /// </summary>
    public enum LivingAreaType
    {
        Camp,  //营地
        Faction,  //帮派
        City,     //城市
        Cave,    //洞窟
    }



    /// <summary>
    /// 建筑物
    /// </summary>
    public class BuildingObject
    {
        public string Key { get; set; }                         //建筑物key
        public string Name { get; set; }                       //名称                                           
        public string Description { get; set; }                 //说明
        public int BuildingLevel { get; set; }                  //建筑物等级
        public BuildingStatus Status { get; set; }                 //建筑物状态                      
        public BuildingType Type { get; set; }                   //建筑物类型    
        public int DurableValue { get; set; }                   //建筑物耐久  百分比                                                                                                        
        public int OwnId { get; set; }                          //拥有者Id                                        
        public string BuildingFeaturesIds { get; set; }         //建筑物特征
        public string MarkIds { get; set; }                     //建筑物词缀
        public string ModelPath { get; set; }                   //模型位置
        public BuildingObject() { }

        public BuildingObject(string key, string name, string description, int buildingLevel, BuildingStatus status, BuildingType type,
            int durableValue, int ownId, string buildingFeaturesIds, string markIds, string modelPath)
        {
            this.Key = key;
            this.Name = name;
            this.Description = description;
            this.BuildingLevel = buildingLevel;
            this.Status = status;
            this.Type = type;
            this.DurableValue = durableValue;
            this.OwnId = ownId;
            this.BuildingFeaturesIds = buildingFeaturesIds;
            this.MarkIds = markIds;
            this.ModelPath = modelPath;
        }
    }

    //建筑物状态
    public enum BuildingStatus
    {
        None,                   //空地
        Normal,                 //正常
        UnderConstruction,     //建筑中
    }
    /// <summary>
    /// 建筑物类型
    /// </summary>
    public enum BuildingType
    {
        Workout,                //锻炼
        Rest                   //休息

    }

    public class BuildingFeatures
    {
        public string Name;
        public string Description;
    }

    /// <summary>
    /// 设施类型
    /// </summary>
    public enum BuildingFeatureType
    {

    }

    public class LivingAreaText
    {
        public string Name;
        public string Description;

        public LivingAreaText(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }



    public class LivingAreaSystem : ComponentSystem
    {

        public bool CurShowUi = false;

        //UI
        private LivingAreaTitleWindow _livingAreaTitle;

        struct LivingAreaGroup
        {
            public readonly int Length;
            public ComponentDataArray<LivingArea> LivingAreaNode;
            public ComponentArray<Transform> LivingAreaPositon;
        }
        [Inject]
        private LivingAreaGroup _livingAreas;

        private Dictionary<int, LivingAreaText> _livingAreaTextDic = new Dictionary<int, LivingAreaText>();

        public static void SetupComponentData(EntityManager entityManager)
        {
            //LivingArea[] livingAreaCom = GameObject.Find("StrategyManager").GetComponentsInChildren<LivingArea>();
            //List<LivingAreaData> livingAreaDatas = SqlData.GetAllDatas<LivingAreaData>();
            //for (int i = 0; i < livingAreaCom.Length; i++)
            //{
            //    for (int j = 0; j < livingAreaDatas.Count; j++)
            //    {
            //        if (livingAreaCom[i].Id == livingAreaDatas[j].Id)
            //        {
            //            livingAreaCom[i].Name = livingAreaDatas[j].Name;
            //            livingAreaCom[i].Description = livingAreaDatas[j].Description;
            //            livingAreaCom[i].PersonNumber = livingAreaDatas[j].PersonNumber;
            //            livingAreaCom[i].CurLevel = livingAreaDatas[j].LivingAreaLevel;
            //            livingAreaCom[i].MaxLevel = livingAreaDatas[j].LivingAreaMaxLevel;
            //            livingAreaCom[i].Type = (LivingAreaType)livingAreaDatas[j].LivingAreaType;
            //            livingAreaCom[i].Money = livingAreaDatas[j].Money;
            //            livingAreaCom[i].MoneyMax = livingAreaDatas[j].MoneyMax;
            //            livingAreaCom[i].Iron = livingAreaDatas[j].Iron;
            //            livingAreaCom[i].IronMax = livingAreaDatas[j].IronMax;
            //            livingAreaCom[i].Wood = livingAreaDatas[j].Wood;
            //            livingAreaCom[i].WoodMax = livingAreaDatas[j].WoodMax;
            //            livingAreaCom[i].Food = livingAreaDatas[j].Food;
            //            livingAreaCom[i].FoodMax = livingAreaDatas[j].FoodMax;
            //            livingAreaCom[i].DefenseStrength = livingAreaDatas[j].DefenseStrength;
            //            livingAreaCom[i].StableValue = livingAreaDatas[j].StableValue;
            //            livingAreaCom[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(livingAreaDatas[j].BuildingInfoJson);
            //        }
            //    }
            //}
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            Debug.Log("LivingAreaSystem Start");
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
        }

        protected override void OnUpdate()
        {

            for (int i = 0; i < _livingAreas.Length; i++)
            {
                var livingArea = _livingAreas.LivingAreaNode[i];
                if (livingArea.IsInternal == 1)
                {
                    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);

                }
            }

            if (CurShowUi == false)
            {
                if (_livingAreaTextDic.Count != _livingAreas.Length)    //需要更新数据
                {
                    ChangeText();
                }
                string[] names = new string[_livingAreas.Length];
                Vector3[] points = new Vector3[_livingAreas.Length];
                for (int i = 0; i < _livingAreas.Length; i++)
                {
                    var la = _livingAreas.LivingAreaNode[i];
                    names[i] = _livingAreaTextDic[la.Id].Name;
                    points[i] = _livingAreas.LivingAreaPositon[i].position;
                }

                if (_livingAreaTitle)
                {
                    ShowWindowData data = new ShowWindowData();
                    data.contextData = new WindowContextLivingAreaData(names, points);
                    _livingAreaTitle.ShowWindow(data.contextData);
                }
                else
                {
                    ShowWindowData data = new ShowWindowData();
                    data.contextData = new WindowContextLivingAreaData(names, points);
                    _livingAreaTitle = (LivingAreaTitleWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow, data);
                }
                CurShowUi = true;
            }


        }

        private void ChangeText()
        {
            Debuger.Log("LivingAreaText Change");
            _livingAreaTextDic.Clear();
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                var la = _livingAreas.LivingAreaNode[i];

                LivingAreaData data = SqlData.GetDataId<LivingAreaData>(la.Id);
                _livingAreaTextDic.Add(la.Id, new LivingAreaText(data.Name, data.Description));
            }
        }


        public LivingAreaWindowCD GetUiData(int id)
        {
            LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
            for (int i = 0; i < _livingAreas.Length; i++)
            {
                if (_livingAreas.LivingAreaNode[i].Id == id)
                {
                    livingAreaWindowCd.Id = id;

                    LivingAreaData data = SqlData.GetDataId<LivingAreaData>(id);
                    livingAreaWindowCd.LivingAreaName = data.Name;
                    livingAreaWindowCd.Description = data.Description;
                    return livingAreaWindowCd; 
                }
            }
            return livingAreaWindowCd;
        }



        ///// <summary>
        ///// 获取指定ID的LivingArea
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public LivingArea GetLivingArea(int id)
        //{
        //    for (int i = 0; i < LivingAreas.Count; i++)
        //    {
        //        if (LivingAreas[i].Id == id)
        //        {
        //            return LivingAreas[i];
        //        }
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// 更新所属
        ///// </summary>
        //public void LivingAreasChangeOwn()
        //{

        //}


        //public void ChangeLivingAreaState()
        //{
        //    //初始化LivingAreas
        //    for (int i = 0; i < LivingAreas.Count; i++)
        //    {

        //        //LivingAreas[i].Value = SqlData.GetDataId<LivingAreaData>(LivingAreas[i].Id);
        //        //LivingAreas[i].BuildingObjects = JsonConvert.DeserializeObject<BuildingObject[]>(LivingAreas[i].Value.BuildingInfoJson);

        //        //？
        //        LivingAreaState[] groups = new LivingAreaState[3];
        //        groups[0] = new LivingAreaState(1, "1", "", 10, null);
        //        groups[1] = new LivingAreaState(2, "1", "", 10, null);
        //        groups[2] = new LivingAreaState(3, "1", "", 10, null);
        //        LivingAreas[i].Groups = groups;
        //    }
        //}


        ///// <summary>
        ///// 实例,构造这个LivingArea所有信息
        ///// </summary>
        ///// <param name="node"></param>
        //public void InstanceLivingArea(LivingArea node)
        //{
        //}

        //public void ChangeLivingAreas()
        //{

        //}

        //public void EnterLivingAreas(LivingArea livingAreaNode, Biological biological)
        //{
        //    //pow>rece>guanxi
        //    if (biological == null)
        //    {
        //        Debuger.Log("Value 为空");
        //        return;
        //    }
        //    switch (biological.RaceType)
        //    {
        //        case RaceType.Elf:

        //            break;
        //        case RaceType.Human:

        //            break;
        //    }
        //    //if (livingAreaNode.Value.PowerId == biological.PowerId)
        //    //{
        //    //}
        //    biological.CurWhereStatus = WhereStatus.City;
        //}

        //public void EnterLivingAreas(LivingArea livingAreaNode, List<Biological> biologicals)
        //{

        //}

        //public void ChangeCamera(ViewStatus convertView)
        //{
        //    if (CurViewStatus == convertView)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        if (CurViewStatus == ViewStatus.CityMainView && convertView == ViewStatus.WorldMapView)
        //        {
        //            Cur3DMainCamera.gameObject.SetActive(false);
        //            Cur3DMainCamera = Main3Dcamera;
        //            Cur3DMainCamera.gameObject.SetActive(true);

        //        }
        //        else if (CurViewStatus == ViewStatus.WorldMapView && convertView == ViewStatus.CityMainView)
        //        {
        //            Cur3DMainCamera.gameObject.SetActive(false);
        //            Cur3DMainCamera = LivingfAreaCamera;
        //            Cur3DMainCamera.gameObject.SetActive(true);

        //        }

        //    }




        //}






        //public void WorldMapViewToCityMainView()
        //{

        //    ShowWindowData data = new ShowWindowData();
        //    data.contextData = new WindowContextLivingAreaNodeData(LivingAreaTarget);
        //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, data);

        //}

        //public void CityMainViewToWorldMapView()
        //{
        //    UICenterMasterManager.Instance.CloseWindow(WindowID.LivingAreaMainWindow);
        //}



        ///// 进入生活区
        ///// </summary>
        //public void LivingAreaEnter(LivingArea livingArea)
        //{
        //    if (livingArea == null)
        //    {
        //        Debuger.LogError("进入生活区时，数据为NULL");
        //        return;
        //    }


        //    //先对角色的属性进行检测 是否可以进入城市，1，角色位置是否在城市附近，2 角色与城市势力关系 3角色与城市首领关系 4城市的异常状态 


        //    //关闭当前开启的界面 ---

        //    UICenterMasterManager.Instance.CloseWindow(WindowID.LivingAreaBasicWindow);

        //    ShowWindowData showWindowData = new ShowWindowData();
        //    showWindowData.contextData = new WindowContextLivingAreaNodeData(livingArea);
        //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

        //    //初始化LivingArea

        //    //    StaticValue.Instance.EnterLivingAreaId = livingArea.Id;


        //    //更新相机
        //    Renderer[] renderers = livingArea.LivingAreaM.GetComponentsInChildren<Renderer>();
        //    if (renderers.Length > 0)
        //    {
        //        Bounds mapBounds = renderers[0].bounds;
        //        for (int i = 0; i < renderers.Length; i++)
        //        {
        //            mapBounds.Encapsulate(renderers[i].bounds);
        //        }
        //        LivingfAreaCameraControl.SetBounds(mapBounds);
        //        LivingfAreaCameraControl.SetPosition(mapBounds.center);
        //    }
        //    //  M_LivingArea.EnterLivingArea(livingArea);
        //}


        //#region MouseEvents
        //public void MouseEnter_PlayerMain(Transform tf)
        //{
        //    Debug.Log(tf.name + ">>MouseEnter");
        //}
        //public void MouseExit_PlayerMain(Transform tf)
        //{
        //    Debug.Log(tf.name + ">>MouseExit");
        //}
        //public void MouseOver_PlayerMain(Transform tf)
        //{
        //    Debug.Log(tf.name + ">> MouseOver");
        //}
        //public void Mouse0Click_PlayerMain(Transform tf, Vector3 point)
        //{
        //    Debug.Log(tf.name + ">>Mouse0Click");
        //}
        //public void Mouse1Click_PlayerMain(Transform tf, Vector3 point)
        //{
        //    Debug.Log(tf.name + ">>Mouse1Click");
        //}

        //public void MouseEnter_LivingAreaMain(Transform tf)
        //{
        //    Debug.Log(tf.name + ">>MouseEnter");
        //}
        //public void MouseExit_LivingAreaMain(Transform tf)
        //{
        //    Debug.Log(tf.name + ">>MouseExit");
        //}
        //public void MouseOver_LivingAreaMain(Transform target)
        //{
        //    Debug.Log(target.name + ">> MouseOver");
        //}
        //public void Mouse0Click_LivingAreaMain(Transform target, Vector3 point)
        //{
        //    Debug.Log(target.name + ">>Mouse0Click");
        //    LivingArea node = target.GetComponent<LivingArea>();
        //    _livingAreasSelect.position = node.LivingAreaRender.bounds.center;
        //    //  MessageBoxInstance.Instance.MessageBoxShow("");

        //    //判断逻辑

        //    if (CurPlayer != null)
        //    {
        //        Debuger.Log("Enter LivingAreas");
        //        CurPlayer.transform.position = node.transform.position;
        //        // M_Strategy.InstanceLivingArea(node);

        //        ShowWindowData showWindowData = new ShowWindowData();
        //        showWindowData.contextData = new WindowContextLivingAreaNodeData(node);
        //        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

        //        if (CurPlayer.GroupId == -1)
        //        {
        //            //  M_Strategy.EnterLivingAreas(node, CurPlayer);
        //        }
        //        else
        //        {
        //            //  M_Strategy.EnterLivingAreas(node, M_Biological.GroupsDic[CurPlayer.GroupId].Partners);
        //        }
        //    }
        //}
        //public void Mouse1Click_LivingAreaMain(Transform target, Vector3 point)
        //{
        //    Debug.Log(target.name + ">>Mouse1Click");

        //    ShowWindowData showMenuData = new ShowWindowData();
        //    showMenuData.contextData = new WindowContextExtendedMenu(target.GetComponent<LivingArea>(), point);
        //    UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow, showMenuData);
        //}

        //public void MouseEnter_Terrain(Transform tf)
        //{
        //}

        //public void MouseExit_Terrain(Transform tf)
        //{

        //}

        //public void MouseOver_Terrain(Transform tf)
        //{

        //}

        //public void Mouse0Click_Terrain(Transform tf, Vector3 point)
        //{
        //    CurMouseEffect.transform.position = point;
        //}

        //public void Mouse1Click_Terrain(Transform tf, Vector3 point)
        //{
        //    Debuger.Log(">>>>>>>Terrain");
        //    CurMouseEffect.transform.position = point;
        //    CurPlayer.GetComponent<AICharacterControl>().SetTarget(CurMouseEffect.transform);
        //    NavMeshAgent agent = CurPlayer.GetComponent<NavMeshAgent>();
        //    LineRenderer moveLine = gameObject.GetComponent<LineRenderer>();
        //    if (moveLine == null)
        //    {
        //        moveLine = gameObject.AddComponent<LineRenderer>();
        //    }

        //    //设置路径的点，
        //    //路径  导航。
        //    NavMeshPath path = new NavMeshPath();
        //    agent.CalculatePath(point, path);
        //    //线性渲染设置拐点的个数。数组类型的。
        //    moveLine.positionCount = path.corners.Length;
        //    //线性渲染的拐点位置，数组类型，
        //    agent.SetDestination(point);
        //    moveLine.SetPositions(path.corners);
        //}

        //public void MouseEnter_Biological(Transform tf)
        //{

        //}

        //public void MouseExit_Biological(Transform tf)
        //{

        //}

        //public void MouseOver_Biological(Transform tf)
        //{

        //}

        //public void Mouse0Click_Biological(Transform tf, Vector3 point)
        //{

        //}

        //public void Mouse1Click_Biological(Transform tf, Vector3 point)
        //{

        //}

        //#endregion


        ////显示Message
        //public void MessageShow(string[] values)
        //{
        //}

        //public void MessageShow(string value)
        //{

        //}




        //public void OpenWXCharacterPanelWidow()
        //{
        //    UICenterMasterManager.Instance.ShowWindow(WindowID.WXCharacterPanelWindow);
        //}
    }

}


