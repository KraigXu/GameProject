using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;


public class DemoCharacterScene : MonoBehaviour
{


    public Text NameText;


    public class Biological
    {
        public int Id { get; set; }                              
        public string Surname { get; set; }                      
        public string Name { get; set; }
        public int Sex { get; set; }                            
        public int Age { get; set; }           
        public int LifeValue { get; set; }
        

        public int Power { get; set; }
        public int Spirit { get; set; }
        public int Maneuver { get; set; }
        public int Wisdom { get; set; }
        public int Charm { get; set; }


      //  public int 

        public int AvatarId { get; set; }          
        public int ModelId { get; set; }




    }



    void Start()
    {
        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        StartCoroutine(InitBiologicalData(entityManager));
    }

    void Update()
    {



    }

    IEnumerator InitBiologicalData(EntityManager entityManager)
    {
        Dictionary<int, Entity> familyIdMap = new Dictionary<int, Entity>();
        Dictionary<int, Entity> factionIdMap = new Dictionary<int, Entity>();
        Dictionary<int, Entity> biologicalIdMap = new Dictionary<int, Entity>();
        //--------------------FamilyI
        EntityArchetype familyArchetype = entityManager.CreateArchetype(typeof(Family));
        List<FamilyData> familyData = SQLService.Instance.QueryAll<FamilyData>();
        for (int i = 0; i < familyData.Count; i++)
        {
            Entity family = entityManager.CreateEntity(familyArchetype);
            FamilySystem.CreateFamily(entityManager, family, familyData[i]);
            familyIdMap.Add(familyData[i].Id, family);
        }
        familyData.Clear();
        yield return new WaitForFixedUpdate();
        //----------------------生成Fatction
        EntityArchetype factionArchetype = SystemManager.ActiveManager.CreateArchetype(typeof(Faction));
        List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();

        for (int i = 0; i < factionDatas.Count; i++)
        {
            Entity faction = SystemManager.ActiveManager.CreateEntity(factionArchetype);
            FactionSystem.CreateFaction(entityManager, faction, factionDatas[i]);
            factionIdMap.Add(factionDatas[i].Id, faction);
        }
        factionDatas.Clear();
        yield return new WaitForFixedUpdate();

        //------------------初始化Biological
        List<BiologicalData> biologicalDatas = SQLService.Instance.QueryAll<BiologicalData>();
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];
            Entity entity = entityManager.CreateEntity();
            BiologicalSystem.CreateBiological(entityManager, entity, bData);

            switch (bData.Identity)
            {
                case 0:
                case 1:
                case 2:
                default:
                    SystemManager.Get<EquipmentSystem>().AddEquipment(entity, bData.EquipmentJson);
                    SystemManager.Get<ArticleSystem>().SettingArticleFeature(entity, bData.Id);
                    SystemManager.Get<TechniquesSystem>().SpawnTechnique(entity, bData.Id);
                    break;
            }

            if (bData.FamilyId != 0)
                FamilySystem.AddFamilyCom(entityManager, familyIdMap[bData.FamilyId], entity);

            if (bData.FactionId != 0)
                FactionSystem.AddFactionCom(entityManager, factionIdMap[bData.FactionId], entity);

            if (bData.TeamId != 0)
                // TeamSystem.SetupData(_entityManager, entity);

                biologicalIdMap.Add(biologicalDatas[i].Id, entity);

        }
        biologicalDatas.Clear();
        bData = null;

        yield return new WaitForFixedUpdate();

        List<RelationData> relationDatas = SQLService.Instance.QueryAll<RelationData>();
        for (int i = 0; i < relationDatas.Count; i++)
        {
            if (biologicalIdMap.ContainsKey(relationDatas[i].MainId) &&
                biologicalIdMap.ContainsKey(relationDatas[i].AimsId))
            {
                RelationSystem.AddRealtionValue(biologicalIdMap[relationDatas[i].MainId], biologicalIdMap[relationDatas[i].AimsId]);
            }
            else
            {
                Debug.Log(relationDatas[i].MainId + "-----" + relationDatas[i].AimsId);
            }


        }

        relationDatas.Clear();

        yield return new WaitForFixedUpdate();

        List<CampData> campDatas = SQLService.Instance.QueryAll<CampData>();
        CampData camp;
        for (int i = 0; i < campDatas.Count; i++)
        {
            camp = campDatas[i];
            //hexCoordinates = new HexCoordinates(camp.X, camp.Y);
            //hexCell = hexGrid.GetCell(hexCoordinates);
            //if (hexCell == null)
            //    continue;

            GameObject go = new GameObject();
            HexUnit hexUnit = go.AddComponent<HexUnit>();
            Entity entity = go.AddComponent<GameObjectEntity>().Entity;

            if (camp.ModelId != 0)
            {
                ModelSpawnSystem.SetupComponentData(entity, go);
            }

            //HexUnit hexUnit = Instantiate(StrategyAssetManager.GetHexUnitPrefabs(camp.ModelId));
            //Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;
            //int[] bioid;
            //campDatas[i].Ids.Split(',');
            // TeamSystem.SetupData(_entityManager,entity);
            // hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
        }
        campDatas.Clear();

        yield return new WaitForFixedUpdate();
        Dictionary<int, GameObject> teamMap = new Dictionary<int, GameObject>();
        List<TeamData> teamDatas = SQLService.Instance.QueryAll<TeamData>();
        GameObject teamPrefab = new GameObject("TeamModel");
        teamPrefab.transform.position = Vector3.zero;

        for (int i = 0; i < teamDatas.Count; i++)
        {

            GameObject itemgo = Instantiate(StrategyAssetManager.TemeModelPrefabs);

            GameObjectEntity entityCom = itemgo.gameObject.GetComponent<GameObjectEntity>();

            TeamSystem.SetupData(entityManager, entityCom.Entity, teamDatas[i]);

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
                if (biologicalIdMap.ContainsKey(memberId))
                {
                    teamFixed.Members.Add(biologicalIdMap[memberId]);
                }
            }

            GameStaticData.TeamRunDic.Add(entityCom.Entity, teamFixed);
            teamMap.Add(teamDatas[i].Id, itemgo);
        }

        yield return new WaitForFixedUpdate();


        factionIdMap.Clear();
        biologicalIdMap.Clear();
        familyIdMap.Clear();

    }


    IEnumerator Player()
    {

        ////初始玩家信息
        //OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;
        //if (biologicalIdMap.ContainsKey(openingInfo.PlayerId))
        //{
        //    Entity playerEntity = biologicalIdMap[openingInfo.PlayerId];
        //    PlayerControlSystem.SetupComponentData(entityManager, playerEntity);

        //    OverLookCameraController overLookCamera = Camera.main.gameObject.GetComponent<OverLookCameraController>();
        //    overLookCamera.m_targetPosition = teamMap[openingInfo.TeamId].transform.position;

        //    PlayerController.Instance.PlayerGo = teamMap[openingInfo.TeamId];

        //    //TeamId
        //    //StrategyPlayer.PlayerInit(1, pData.Name, pData.Surname, StrategyAssetManager.GetBiologicalAvatar(1), entity, hexUnit);
        //}
        //else
        //{
        //    Debug.Log("异常错误");
        //}


        ////初始玩家操作
        //GameObject playgo=new GameObject("PlayerController");
        //playgo.AddComponent<PlayerMouseControl>();
        UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow);

        yield return new WaitForFixedUpdate();

        //  SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
        // SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);
        // SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);

        // LivingAreaSystem.SetupComponentData(entityManager, hexGrid);

        //SystemManager.Get<WorldTimeSystem>().SetupValue(true);

        // HexMapCamera.SetTarget(StrategyPlayer.Unit.transform.position);
        //  todo :目前先不初始UI
        //  World.Active.GetOrCreateManager<PlayerMessageUiSystem>().SetupGameObjects();
        //  World.Active.GetOrCreateManager<WorldTimeSystem>().SetupValue(true);
       // factionIdMap.Clear();
       // biologicalIdMap.Clear();
       // familyIdMap.Clear();
        GC.Collect();

        //LoadingViewCom.Close();
    }


}
