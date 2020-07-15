using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;
using GameSystem;
using Newtonsoft.Json;

public class DataLoadEcs : MonoBehaviour
{
    public float Progress;
    public bool IsOver = false;

    public Dictionary<int, Entity> FamilyIdMap = new Dictionary<int, Entity>();
    public Dictionary<int, Entity> FactionIdMap = new Dictionary<int, Entity>();
    public Dictionary<int, Entity> BiologicalIdMap = new Dictionary<int, Entity>();
    public Dictionary<int, GameObject> TeamMap = new Dictionary<int, GameObject>();

    public struct BehaviorData : IComponentData
    {
        
        
        public Vector3 Target;
        public int TargetId;
        public float TimeToLive;
        public Entity TargetEntity;
        public int CreantePositionCode;

        public Vector3 SelfPoint;
        public Vector3 NextPoint;
        public uint BehaviourType;

        public HexCoordinates Target1;
    }

    //public struct PositionInfo
    //{
    //    public Vector3 Position;
    //    public Vector3 Rotation;
    //}


   // private PositionInfo position;


    void Start()
    {
        //position=new PositionInfo()
        //{
        //    Position = new Vector3(10,10,10),
        //    Rotation=new Vector3(0,0,100),
        //};

        //Debug.Log(JsonConvert.SerializeObject(position));
    }





    /// <summary>
    /// 初始化生物数据
    /// </summary>
    private IEnumerator InitBiologicalData()
    {

        //--------------------FamilyI

        EntityArchetype familyArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Family));
        List<FamilyData> familyData = SQLService.Instance.QueryAll<FamilyData>();
        for (int i = 0; i < familyData.Count; i++)
        {
            Entity family = SystemManager.ActiveManager.CreateEntity(familyArchetype);
            FamilySystem.CreateFamily(SystemManager.ActiveManager, family, familyData[i]);
            FamilyIdMap.Add(familyData[i].Id, family);
        }
        familyData.Clear();
        Progress = 100f / 0.1f;
        yield return new WaitForFixedUpdate();

        //----------------------生成Fatction
        EntityArchetype factionArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Faction));
        List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();

        for (int i = 0; i < factionDatas.Count; i++)
        {
            Entity faction = SystemManager.ActiveManager.CreateEntity(factionArchetype);
            FactionSystem.CreateFaction(SystemManager.ActiveManager, faction, factionDatas[i]);
            FactionIdMap.Add(factionDatas[i].Id, faction);
        }
        factionDatas.Clear();
        yield return new WaitForFixedUpdate();
        Progress = 100f / 0.3f;

        //------------------初始化Biological
        List<BiologicalData> biologicalDatas = SQLService.Instance.QueryAll<BiologicalData>();
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];
            Entity entity = SystemManager.ActiveManager.CreateEntity();
            BiologicalSystem.CreateBiological(SystemManager.ActiveManager, entity, bData);

            switch (bData.Identity)
            {
                case 0:
                case 1:
                case 2:
                default:
                    EquipmentSystem.AddEquipment(entity, bData.EquipmentJson, SystemManager.ActiveManager);
                    ArticleSystem.SettingArticleFeature(entity, bData.Id, SystemManager.ActiveManager);
                    TechniquesSystem.SpawnTechnique(entity, bData.Id, SystemManager.ActiveManager);
                    break;
            }

            if (bData.FamilyId != 0)
                FamilySystem.AddFamilyCom(SystemManager.ActiveManager, FamilyIdMap[bData.FamilyId], entity);

            if (bData.FactionId != 0)
                FactionSystem.AddFactionCom(SystemManager.ActiveManager, FactionIdMap[bData.FactionId], entity);

                BiologicalIdMap.Add(biologicalDatas[i].Id, entity);

        }
        biologicalDatas.Clear();
        bData = null;

        yield return new WaitForFixedUpdate();

        List<RelationData> relationDatas = SQLService.Instance.QueryAll<RelationData>();
        for (int i = 0; i < relationDatas.Count; i++)
        {
            if (BiologicalIdMap.ContainsKey(relationDatas[i].MainId) && BiologicalIdMap.ContainsKey(relationDatas[i].AimsId))
            {
                RelationSystem.AddRealtionValue(BiologicalIdMap[relationDatas[i].MainId], BiologicalIdMap[relationDatas[i].AimsId]);
            }
            else
            {
            }
        }

        relationDatas.Clear();

        yield return new WaitForFixedUpdate();

        //List<CampData> campDatas = SQLService.Instance.QueryAll<CampData>();
        //CampData camp;
        //for (int i = 0; i < campDatas.Count; i++)
        //{
        //    camp = campDatas[i];
        //    GameObject go = new GameObject();
        //    HexUnit hexUnit = go.AddComponent<HexUnit>();
        //    Entity entity = go.AddComponent<GameObjectEntity>().Entity;

        //    if (camp.ModelId != 0)
        //    {
        //        ModelSpawnSystem.SetupComponentData(entity, go);
        //    }
        //    //HexUnit hexUnit = Instantiate(StrategyAssetManager.GetHexUnitPrefabs(camp.ModelId));
        //    //Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;
        //    //int[] bioid;
        //    //campDatas[i].Ids.Split(',');

        //    // TeamSystem.SetupData(_entityManager,entity);
        //    // hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
        //}
        //campDatas.Clear();

       
        List<TeamData> teamDatas = SQLService.Instance.QueryAll<TeamData>();
        GameObject teamPrefab = new GameObject("TeamModel");
        teamPrefab.transform.position = Vector3.zero;

        for (int i = 0; i < teamDatas.Count; i++)
        {
            GameObject itemgo = Instantiate(StrategyAssetManager.TemeModelPrefabs);

            GameObjectEntity entityCom = itemgo.gameObject.GetComponent<GameObjectEntity>();

            ModelMoveSystem.SetupData(SystemManager.ActiveManager, entityCom.Entity,teamDatas[i]);

            TeamSystem.SetupData(SystemManager.ActiveManager, entityCom.Entity, teamDatas[i]);

            string[] memberIds = teamDatas[i].MemberIds.Split(';');

            StatusInfo statusInfo = JsonConvert.DeserializeObject<StatusInfo>(teamDatas[i].StatusInfo);
            itemgo.transform.position = statusInfo.Position;
            itemgo.transform.rotation = Quaternion.Euler(statusInfo.Face);
            itemgo.transform.SetParent(teamPrefab.transform);

            TeamFixed teamFixed = new TeamFixed();
            teamFixed.Id = teamDatas[i].Id;
            teamFixed.Transform = itemgo.transform;

            for (int j = 0; j < memberIds.Length; j++)
            {
                int memberId = int.Parse(memberIds[j]);
                if (BiologicalIdMap.ContainsKey(memberId))
                {
                    teamFixed.Members.Add(BiologicalIdMap[memberId]);
                }
            }

            GameStaticData.TeamRunDic.Add(entityCom.Entity, teamFixed);
            TeamMap.Add(teamDatas[i].Id, itemgo);
        }

        yield return new WaitForFixedUpdate();
        
        //  SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
        // SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);
        // SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);

        // LivingAreaSystem.SetupComponentData(entityManager, hexGrid);

        //SystemManager.Get<WorldTimeSystem>().SetupValue(true);
        // HexMapCamera.SetTarget(StrategyPlayer.Unit.transform.position);
        //  World.Active.GetOrCreateManager<PlayerMessageUiSystem>().SetupGameObjects();
        //  World.Active.GetOrCreateManager<WorldTimeSystem>().SetupValue(true);
        OverLoad();
    }


    public void StartLoad()
    {
        IsOver = false;
        Progress = 0;
       // StartCoroutine(InitBiologicalData());
    }

    public void OverLoad()
    {
        IsOver = true;
        Progress = 100;

        //SignalCenter.GameDataLoadOver.Dispatch(this);

    }






}
