using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform TargetTF;
    public GameObject PlayerGo;
    public NavMeshAgent agent;

    private static PlayerController _instance;

    private bool _isInit = false;
    private bool _isCous = true;

    private Entity _playerEntity;
    private OverLookCameraController _overLookCamera;
    private PlayerInfoWindow _infoWin;
    private MessageWindow _message;
    private MenuWindow _menuWindow;

    public static PlayerController Instance
    {
        get { return _instance; }
    }


    void Awake()
    {
        _instance = this;
        //SignalCenter.GameDataLoadOver.AddListener(PlayerInit);

    }
	void Start () {
		
       // SignalCenter.MouseOnMove.AddListener(PlayerOnMouseMove);
	   // SignalCenter.MouseOnRightUp.AddListener(PlayerOnRightClick);
       // SignalCenter.MouseOnLeftUp.AddListener(PlayerOnLeftClick);

	}


    void Update()
    {
        if(_isInit==false) return;

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyTeamTipsWindow);
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            //队伍信息
              UICenterMasterManager.Instance.DestroyWindow(WindowID.StrategyTeamTipsWindow);  

        }

        if (_isCous == true)
        {
            _overLookCamera.m_targetPosition = SystemManager.GetProperty<StatusInfo>(_playerEntity).Position;
         
        }
        //Update Data
        //if (_curCell != _unit.Location)
        //{
        //    _curCell = _unit.Location;
        //    while (CellFeaturesParent.childCount > 0)
        //    {
        //        WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellFeaturesParent.GetChild(0));
        //    }

        //    while (CellPersonsParent.childCount > 0)
        //    {
        //        WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellPersonsParent.GetChild(0));
        //    }

        //    _personUnits.Clear();


        //    switch (_curCell.Elevation)
        //    {

        //    }

        //    switch (_curCell.FarmLevel)
        //    {

        //    }

        //    switch (_curCell.SpecialIndex)
        //    {

        //    }

        //    switch (_curCell.UrbanLevel)
        //    {

        //    }




        //    //根据cell显示内容，解析cell


        //    //_curCell.TerrainTypeIndex
        //    //_curCell.Elevation
        //    //_cur

        //    int cellType = 3;

        //    CellTypeImage.overrideSprite = StrategyAssetManager.GetCellTypeSprites(cellType);
        //    CellTypeTxt.text = "";
        //    CellUrbanTxt.text = _unit.Location.UrbanLevel.ToString();
        //    CellFarmTxt.text = _unit.Location.FarmLevel.ToString();

        //    switch (_unit.Location.FarmLevel)
        //    {
        //        case 0:

        //            break;
        //    }
        //    switch (_unit.Location.PlantLevel)
        //    {
        //        case 0:
        //            CellPlantTxt.text = "树木稀少";
        //            break;
        //        case 1:
        //            CellPlantTxt.text = "草原";
        //            break;
        //        case 2:
        //            CellPlantTxt.text = "树林";
        //            break;
        //        case 3:
        //            CellPlantTxt.text = "森林";
        //            break;
        //        case 4:
        //            CellPlantTxt.text = "丛林";
        //            break;
        //        default:
        //            break;
        //    }

        //    CellPlantTxt.text = _unit.Location.PlantLevel.ToString();


        //    if (_unit.Location.SpecialIndex >= 0)
        //    {
        //        switch (_unit.Location.SpecialIndex)
        //        {
        //            case 1:  //城市
        //                {

        //                    LivingArea livingArea = SystemManager.GetProperty<LivingArea>(_unit.Location.Entity);

        //                    UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
        //                    featureUi.Init(GameStaticData.CityRunDataDic[livingArea.Id].Name, StrategyAssetManager.GetCellFeatureSpt(livingArea.ModelId), _unit.Location.Entity, CityOpenEntity);
        //                }
        //                break;
        //            case 2: //帮派
        //                {

        //                    Collective collective = SystemManager.GetProperty<Collective>(_unit.Location.Entity);

        //                    UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
        //                    //featureUi.Init(GameStaticData.CityRunDataDic);

        //                    //UiCellFeature featureUi=WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature,CellPersonsParent).GetComponent<UiCellFeature>();
        //                    //featureUi.Init(GameStaticData.);

        //                }
        //                break;
        //            case 3:  //遗迹
        //                {

        //                }
        //                break;
        //            case 4: //事件
        //                {

        //                }
        //                break;
        //            case 5: //奖励
        //                {

        //                }
        //                break;
        //            case 6: //修炼地
        //                {
        //                }
        //                break;



        //        }

        //    }

        //    SystemManager.Get<BiologicalSystem>().GetPoint(ref _personUnits, _curCell.coordinates.X, _curCell.coordinates.Z);

        //    for (int i = 0; i < _personUnits.Count; i++)
        //    {
        //        if (_personUnits[i] == Define.Player.Unit)
        //            continue;

        //        Entity entity = _personUnits[i].Entity;

        //        if (SystemManager.Contains<Biological>(entity))
        //        {
        //            Biological biological = SystemManager.GetProperty<Biological>(entity);
        //            BiologicalFixed biologicalFixed = GameStaticData.BiologicalDictionary[entity];


        //            RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiPersonButton, CellPersonsParent);
        //            BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
        //            bui.Avatar = StrategyAssetManager.GetBiologicalAvatar(biological.AvatarId);

        //            bui.PersonName = biologicalFixed.Surname + biologicalFixed.Name;
        //            bui.Entity = entity;
        //        }

        //    }
        //}
        //else
        //{

        //}



    }


    void OnDestroy()
    {
       // SignalCenter.MouseOnMove.RemoveListener(PlayerOnMouseMove);
       // SignalCenter.MouseOnMove.RemoveListener(PlayerOnLeftClick);
    }

    private void PlayerOnMouseMove(MouseInput input)
    {
        TargetTF.position = input.touchPoint;
    }

    
    private void PlayerOnLeftClick(MouseInput input)
    {
        if (PlayerGo != null)
        {
            agent = PlayerGo.GetComponent<NavMeshAgent>();
            agent.destination = input.touchPoint;
        }
    }

    private void PlayerOnRightClick(MouseInput input)
    {
        if (PlayerGo != null)
        {
            PlayerGo.gameObject.transform.position = input.touchPoint;
               NavMeshAgent agent = PlayerGo.GetComponent<NavMeshAgent>();
               agent.destination = input.touchPoint;
        }
    }



    private void PlayerInit(DataLoadEcs ecs)
    {
        //初始玩家信息
        OpeningInfo openingInfo = GameSceneInit.CurOpeningInfo;

        if (ecs.BiologicalIdMap.ContainsKey(openingInfo.PlayerId))
        {
            _playerEntity = ecs.BiologicalIdMap[openingInfo.PlayerId];

            SystemManager.ActiveManager.AddComponentData(_playerEntity, new PlayerMember());

            _overLookCamera = Camera.main.gameObject.GetComponent<OverLookCameraController>();
            _overLookCamera.m_targetSize = 40;

            _infoWin = (PlayerInfoWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);
            _message = (MessageWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
            _menuWindow = (MenuWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
            //  StrategyPlayer.PlayerInit(1, pData.Name, pData.Surname, StrategyAssetManager.GetBiologicalAvatar(1), entity, hexUnit);

            _isInit = true;
        }
        else
        {
            Debuger.Log("Biological表内没有主Id数据");
            _isInit = false;
        }
    }



    void OnDrawGizmos()
    {
        if (agent == null) return;
        var path = agent.path;
        // color depends on status
        Color c = Color.white;
        switch (path.status)
        {
            case UnityEngine.AI.NavMeshPathStatus.PathComplete: c = Color.white; break;
            case UnityEngine.AI.NavMeshPathStatus.PathInvalid: c = Color.red; break;
            case UnityEngine.AI.NavMeshPathStatus.PathPartial: c = Color.yellow; break;
        }
        // draw the path
        for (int i = 1; i < path.corners.Length; ++i)
            Debug.DrawLine(path.corners[i - 1], path.corners[i], c);
    }
}
