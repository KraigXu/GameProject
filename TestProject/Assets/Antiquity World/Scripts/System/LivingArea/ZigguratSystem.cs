using System.Collections;
using System.Collections.Generic;

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
            var livingArea = _data.LivingAreas[i];
            var ziggurat = _data.Ziggurats[i];
            var hexcell = _data.HexCells[i];

            _zigguratTitleWindow.Change(_data.LivingAreas[i], _data.Ziggurats[i], _data.HexCells[i]);
        }

    }

    public static void AddZiggurat(EntityManager entityManager, LivingAreaData data, HexCell cell)
    {

        entityManager.AddComponentData(cell.Entity, new Ziggurat
        {
            Time = 30,
        });

        entityManager.AddComponentData(cell.Entity, new LivingArea
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

        entityManager.AddComponentData(cell.Entity, new Collective()
        {
            Id = data.Id,
            CollectiveClassId = 1,
            Cohesion = 1
        });

        //_entityManager.AddComponentData(cell.Entity, );
        //LivingAreaSystem.LivingAreaAddBuilding(entity, data.BuildingInfoJson);

    }

}
