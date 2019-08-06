using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// 迷宫
/// </summary>
public class ZigguratSystem : ComponentSystem
{

    struct Data
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentDataArray<Ziggurat> Ziggurats;
        public ComponentDataArray<LivingArea> LivingAreas;
        public ComponentArray<HexCell> HexCells;
    }
    [Inject]
    private Data _data;
    private EntityManager _entityManager;

    private ZigguratTitleWindow _zigguratTitleWindow;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        _entityManager = World.Active.GetOrCreateManager<EntityManager>();
    }

    protected override void OnUpdate()
    {

        if (_zigguratTitleWindow == null)
        {
            if (UICenterMasterManager.Instance.GetGameWindow(WindowID.ZigguratTitleWindow) == null)
            {
                _zigguratTitleWindow = UICenterMasterManager.Instance.ShowWindow(WindowID.ZigguratTitleWindow).GetComponent<ZigguratTitleWindow>();
            }
            else
            {
                _zigguratTitleWindow = (ZigguratTitleWindow)UICenterMasterManager.Instance.GetGameWindow(WindowID.ZigguratTitleWindow);
            }
        }

        for (int i = 0; i < _data.Length; i++)
        {
            _zigguratTitleWindow.Change(_data.LivingAreas[i], _data.Ziggurats[i], _data.HexCells[i]);
        }


    }

    public static void OrganizationColliderEnter(GameObject go, Collider other)
    {

    }

    public static void OrganizationColliderExit(GameObject go, Collider other)
    {

    }




    public void AddZiggurat(LivingAreaData data, HexCell cell)
    {

        _entityManager.AddComponentData(cell.Entity, new Ziggurat
        {
        });

        _entityManager.AddComponentData(cell.Entity, new LivingArea
        {
            Id = data.Id,
            PersonNumber = data.PersonNumber,
            Type = (LivingAreaType)data.LivingAreaType,
            Money = data.Money,
            MoneyMax = data.MoneyMax,
            Iron = data.Iron,
            IronMax = data.IronMax,
            Wood = data.Wood,
            WoodMax = data.WoodMax,
            Food = data.Food,
            FoodMax = data.FoodMax,
            DefenseStrength = data.DefenseStrength,
            StableValue = data.StableValue
        });

        _entityManager.AddComponentData(cell.Entity, new Collective()
        {
            Id = data.Id,
            CollectiveClassId = 1,
            Cohesion = 1
        });

        //LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

    }

}
