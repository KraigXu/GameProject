using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

public class StrategyActionSystem : ComponentSystem
{

    struct Data
    {
        public readonly int Length;


        public ComponentDataArray<Biological> Biological;

        public ComponentArray<HexUnit> HexUnit;
        public ComponentArray<Transform> Transforms;

        //   public ComponentDataArray<BodyProperty> Body;
        //   public ComponentDataArray<BehaviorData> Behavior;
    }

    [Inject]
    private BiologicalSystem _data;

    private EntityManager _entityManager;
    private TipsWindow _tipsWindow;

    public static EntityArchetype BiologicalArchetype;

    private Dictionary<Entity, BiologicalSystem.ComponentGroup> ComponentDic = new Dictionary<Entity, BiologicalSystem.ComponentGroup>();




    /// <summary>
    /// 缓存组件
    /// </summary>
    public void InitComponent(GameObject go)
    {

    }

    protected override void OnUpdate()
    {
    }
}
