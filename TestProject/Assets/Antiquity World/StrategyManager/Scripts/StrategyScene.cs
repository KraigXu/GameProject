using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
using Manager;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// StrategyScene主脚本，控制整个场景的的生命周期
/// </summary>
public class StrategyScene : MonoBehaviour
{
    private static StrategyScene _instance;

    public static StrategyScene Instance
    {
        get { return _instance; }
    }

    public bool IsInitOver = false;

    public StrategyPlayer Player;

    public Camera MainCamera;
    public Camera FixedCamera;
    public Camera UiCamera;
    public GameObject go;

    public GameObject ArticleInfoPerfab;

    public bool IsEdit = false;

    public GameObject EditUi1;

    public GameObject GameUi1;
    public GameObject GameUi2;

    //--------

    //---------Map
    public HexGrid hexGrid;
    public HexMapGenerator mapGenerator;

    //---------Message
    public Canvas messageCanvas;
    public LoadingView loadingViewCom;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        StartCoroutine(StrategySceneInit());
    }

    IEnumerator StrategySceneInit()
    {

#if UNITY_EDITOR
        Debuger.EnableLog = true;
        GameSceneInit.CurOpeningInfo.TestValue();
#endif
        GameSceneInit.InitializeWithScene();

        InitMapInfo();
        InitGameData();
        UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);

        // UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);

         UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);

        // UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);
        // UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);

       // GameStaticData.BiologicalNodes[1].gameObject.name="PlayerNode";
        HexMapCamera.SetTarget(GameStaticData.BiologicalNodes[1].position);
       // StrategyCameraManager.Instance.SetTarget(new Vector3(-54.42019f, 50.3085f, 40.11046f));

        loadingViewCom.Close();

        yield return null;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.BackQuote))
        {
            IsEdit = !IsEdit;
        }

        if (IsEdit)
        {
            EditUi1.SetActive(true);

            GameUi1.SetActive(false);
            GameUi2.SetActive(false);
        }
        else
        {
            EditUi1.SetActive(false);

            GameUi1.SetActive(true);
            GameUi2.SetActive(true);
        }

    }

    void InitMapInfo()
    {
        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;

        hexGrid.seed = openingInfo.Mapseed;

        if (openingInfo.IsEditMode == true)
        {
            if (openingInfo.GenerateMaps)
            {
                mapGenerator.GenerateMap(openingInfo.Mapx, openingInfo.Mapz, openingInfo.Wrapping);
            }
            else
            {
                hexGrid.CreateMap(openingInfo.Mapx, openingInfo.Mapz, openingInfo.Wrapping);
            }
            HexMapCamera.ValidatePosition();
        }
        else
        {
            //加载地图
            Debuger.Log(openingInfo.MapFilePath);
            using (BinaryReader reader = new BinaryReader(File.OpenRead(openingInfo.MapFilePath)))
            {
                int header = reader.ReadInt32();
                if (header <= openingInfo.MapFileVersion)
                {
                    hexGrid.Load(reader, header);
                    HexMapCamera.ValidatePosition();
                }
                else
                {
                    Debug.LogWarning("Unknown map format " + header);
                }
            }
        }



    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    void InitGameData()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        HexCoordinates hexCoordinates;
        HexCell hexCell;


        //------------初始化Living
        List<CellTypeData> cellTypeDatas = SQLService.Instance.QueryAll<CellTypeData>();
        for (int i = 0; i < cellTypeDatas.Count; i++)
        {
            GameStaticData.CellTypeName.Add(cellTypeDatas[i].Id,cellTypeDatas[i].Name);
            GameStaticData.CellTypeSprite.Add(cellTypeDatas[i].Id, Resources.Load<Sprite>(cellTypeDatas[i].Sprite));
        }


        List<LivingAreaData> datas = SQLService.Instance.QueryAll<LivingAreaData>();
        for (int i = 0; i < datas.Count; i++)
        {
            var data = datas[i];
            hexCoordinates = new HexCoordinates(data.PositionX, data.PositionZ);

            hexCell = hexGrid.GetCell(hexCoordinates);
            if (hexCell == null)
                continue;
            hexCell.SpecialIndex = data.SpecialIndex;

            switch (data.SpecialIndex)
            {
                case 1:  
                    SystemManager.Get<CitySystem>().AddCity(data, hexCoordinates);
                    break;
                case 2:
                    SystemManager.Get<OrganizationSystem>().AddOrganization(data, hexCoordinates);
                    break;
                case 3:  //Ziggurat
                    SystemManager.Get<ZigguratSystem>().AddOrganization(hexCoordinates);
                    break;
            }
            GameStaticData.LivingAreaName.Add(datas[i].Id, datas[i].Name);
            GameStaticData.LivingAreaDescription.Add(datas[i].Id, datas[i].Description);
        }
        //------------------初始化Biological
        List<BiologicalAvatarData> biologicalAvatarDatas = SQLService.Instance.QueryAll<BiologicalAvatarData>();
        for (int i = 0; i < biologicalAvatarDatas.Count; i++)
        {
            GameStaticData.BiologicalAvatar.Add(biologicalAvatarDatas[i].Id, Resources.Load<Sprite>(biologicalAvatarDatas[i].Path));
        }

        List<BiologicalModelData> biologicalModelDatas = SQLService.Instance.QueryAll<BiologicalModelData>();
        for (int i = 0; i < biologicalModelDatas.Count; i++)
        {
            GameStaticData.BiologicalPrefab.Add(biologicalModelDatas[i].Id, Resources.Load<GameObject>(biologicalModelDatas[i].Path));
        }

        List<BiologicalData> biologicalDatas = SQLService.Instance.SimpleQuery<BiologicalData>(" Id<>?", 1);
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];
            hexCoordinates = new HexCoordinates(bData.X, bData.Z);
            hexCell = hexGrid.GetCell(hexCoordinates);
            if (hexCell == null)
                continue;

            HexUnit hexUnit = Object.Instantiate(StrategyStyle.Instance.HexUnitPrefabs[bData.ModelId]);
            hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
            Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;

            switch (bData.Identity)
            {
                case 1:
                    SystemManager.Get<BiologicalSystem>().AddBiological(bData, entity);
                    break;
                default:
                    Debug.Log("Index 为" + bData.Identity + ">>>>的种类未增加");
                    break;
            }

            if (GameStaticData.BiologicalNameDic.ContainsKey(bData.Id) == false)
            {
                GameStaticData.BiologicalNameDic.Add(bData.Id, bData.Name);
                GameStaticData.BiologicalSurnameDic.Add(bData.Id, bData.Surname);
                GameStaticData.BiologicalDescription.Add(bData.Id, bData.Description);
                GameStaticData.BiologicalNodes.Add(bData.Id, hexUnit.transform);
            }
        }

        //-----------------------------Player
        BiologicalData player = SQLService.Instance.QueryUnique<BiologicalData>(" Id=?", 1);

        hexCoordinates = new HexCoordinates(player.X, player.Z);
        hexCell = hexGrid.GetCell(hexCoordinates);
        HexUnit playerUnit = Object.Instantiate(StrategyStyle.Instance.HexUnitPrefabs[player.ModelId]);
        hexGrid.AddUnit(playerUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
        Entity pentity = playerUnit.GetComponent<GameObjectEntity>().Entity;

        if (player.Identity == 1)
        {
            SystemManager.Get<BiologicalSystem>().AddBiological(player, pentity);
            SystemManager.Get<EquipmentSystem>().AddEquipment(pentity, player.EquipmentJson);
            SystemManager.Get<ArticleSystem>().AddArticles(pentity, player.ArticleJson);
        }

        if (GameStaticData.BiologicalNameDic.ContainsKey(player.Id) == false)
        {
            GameStaticData.BiologicalNameDic.Add(player.Id, player.Name);
            GameStaticData.BiologicalSurnameDic.Add(player.Id, player.Surname);
            GameStaticData.BiologicalDescription.Add(player.Id, player.Description);
            GameStaticData.BiologicalNodes.Add(player.Id, playerUnit.transform);
        }

        Player = new StrategyPlayer()
        {
            PlayerId = 1,
            AvatarSprite = GameStaticData.BiologicalAvatar[1],
            Entity = pentity,
            Name = GameStaticData.BiologicalNameDic[1],
            SurName = GameStaticData.BiologicalSurnameDic[1],
            Unit = playerUnit

        };

        //---------------------------ArticleSystem

         //SystemManager.Get<ArticleSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<TechniquesSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<FactionSystem>().SetupComponentData(entityManager);

        //SystemManager.Get<FamilySystem>().SetupComponentData(entityManager);
        Debug.Log(">>System Over");


    }


    /// <summary>
    /// 删除开局UI
    /// </summary>
    public void RemoveStartUi()
    {
        UICenterMasterManager.Instance.DestroyWindow(WindowID.WorldTimeWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MenuWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.PlayerInfoWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MessageWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.MapWindow);
        UICenterMasterManager.Instance.DestroyWindow(WindowID.LivingAreaTitleWindow);

    }

}
