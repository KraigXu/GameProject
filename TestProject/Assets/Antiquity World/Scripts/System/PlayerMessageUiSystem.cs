using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

public class PlayerMessageUiSystem : ComponentSystem
{

    struct Data
    {
        public readonly int Length;
        public ComponentDataArray<PlayerInput> Input;
        public ComponentArray<HexUnit> Unit;
    }

    private MessageWindow _message;
    private PlayerInfoWindow _playerInfoWindow;
    private MenuWindow _menuWindow;
    public void SetupGameObjects()
    {
        _message=(MessageWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
        _playerInfoWindow = (PlayerInfoWindow) UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);
        _menuWindow = (MenuWindow) UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
    }

    protected override void OnUpdate()
    {
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
}
