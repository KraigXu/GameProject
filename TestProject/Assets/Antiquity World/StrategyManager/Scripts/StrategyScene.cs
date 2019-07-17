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

    public StrategyPlayer Player;

    public Camera MainCamera;
    public Camera BuildCamera;

    public GameObject RunTimeUI;
    public GameObject FixedUI;
    public GameObject EditUI;

    public bool IsEdit = false;

    //---------Map
    public HexGrid hexGrid;
    public HexMapGenerator mapGenerator;

    //---------Message
    public Canvas messageCanvas;
    public LoadingView loadingViewCom;

    //------------Window
    public PlayerInfoWindow PlayerInfoView;


    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        StartCoroutine(StrategySceneInit());
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

    void InitModel()
    {
        //------------ModelController

        List<ModelFileData> modelFileDatas = SQLService.Instance.QueryAll<ModelFileData>();

        ModelController.Instance.ModelFileDatas = modelFileDatas;

        StartCoroutine(ModelController.Instance.ReadModelFileData());

    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    void InitGameData()
    {

        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        HexCoordinates hexCoordinates;
        HexCell hexCell;


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
        }
        //------------------初始化Biological

        List<BiologicalData> biologicalDatas = SQLService.Instance.SimpleQuery<BiologicalData>(" Id<>?", 1);
        BiologicalData bData;
        for (int i = 0; i < biologicalDatas.Count; i++)
        {
            bData = biologicalDatas[i];
            hexCoordinates = new HexCoordinates(bData.X, bData.Z);
            hexCell = hexGrid.GetCell(hexCoordinates);
            if (hexCell == null)
                continue;

            HexUnit hexUnit = Object.Instantiate(StrategyAssetManager.GetHexUnitPrefabs(bData.ModelId));
            hexGrid.AddUnit(hexUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
            Entity entity = hexUnit.GetComponent<GameObjectEntity>().Entity;

            switch (bData.Identity)
            {
                case 1:
                    SystemManager.Get<BiologicalSystem>().AddBiological(bData, entity);

                    //SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);
                    break;
                default:
                    Debug.Log("Index 为" + bData.Identity + ">>>>的种类未增加");
                    break;
            }
        }

        //-----------------------------Player
        BiologicalData player = SQLService.Instance.QueryUnique<BiologicalData>(" Id=?", 1);

        hexCoordinates = new HexCoordinates(player.X, player.Z);
        hexCell = hexGrid.GetCell(hexCoordinates);
        HexUnit playerUnit = Object.Instantiate(StrategyAssetManager.GetHexUnitPrefabs(player.ModelId));
        hexGrid.AddUnit(playerUnit, hexCell, UnityEngine.Random.Range(0f, 360f));
        Entity pentity = playerUnit.GetComponent<GameObjectEntity>().Entity;

        if (player.Identity == 1)
        {
            SystemManager.Get<BiologicalSystem>().AddBiological(player, pentity);
            SystemManager.Get<EquipmentSystem>().AddEquipment(pentity, player.EquipmentJson);
            SystemManager.Get<ArticleSystem>().SettingArticleFeature(pentity, player.Id);
            SystemManager.Get<TechniquesSystem>().SpawnTechnique(pentity,player.Id);
            SystemManager.Get<FightingSystem>().AddFighting(pentity);
        }

        Player = new StrategyPlayer()
        {
            PlayerId = 1,
            AvatarSprite = StrategyAssetManager.GetBiologicalAvatar(1),
            Entity = pentity,
            Name = player.Name,
            SurName = player.Surname,
            Unit = playerUnit
        };

        //---------------------------ArticleSystem
        //SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<RelationSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<SocialDialogSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<PrestigeSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<FactionSystem>().SetupComponentData(entityManager);
        //SystemManager.Get<FamilySystem>().SetupComponentData(entityManager);
        Debug.Log(">>System Over");


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
        InitModel();

        //延时开启Window， 因为可能存在数据未完成的情况
        StartCoroutine(WindowSyncOpen());

        yield return null;
    }


    IEnumerator WindowSyncOpen()
    {
        yield return new  WaitForFixedUpdate();

        UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);
        // UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);
        HexMapCamera.SetTarget(Player.Unit.transform.position);

        loadingViewCom.Close();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.BackQuote))
        {
            IsEdit = !IsEdit;
        }

        if (IsEdit)
        {
            EditUI.SetActive(true);

            RunTimeUI.SetActive(false);
            FixedUI.SetActive(false);
        }
        else
        {
            EditUI.SetActive(false);

            RunTimeUI.SetActive(true);
            FixedUI.SetActive(true);
        }

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



    /// <summary>
    /// 退出地图模式
    /// </summary>
    public void ExitMapModel()
    {
        UICenterMasterManager.Instance.CloseWindow(WindowID.MessageWindow);
        Instance.MainCamera.enabled = false;

        PlayerInfoView.Isflag = false;

    }


    /// <summary>
    /// 进入地图模式
    /// </summary>
    public void EnterMapModel()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);

        Instance.MainCamera.enabled = true;

        PlayerInfoView.Isflag = true;
    }

    public void EnterBuildModel()
    {

        BuildCamera.enabled = true;

    }

    public void ExitBuildModel()
    {
        BuildCamera.enabled = false;
    }

}
