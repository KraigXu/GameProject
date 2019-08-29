using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    public class LiningAreaEnterBarrier : BarrierSystem
    {
    }

    public class LivingAreaInteractionSystem : JobComponentSystem
    {
        struct BiologicalData
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
        }
        [Inject]
        BiologicalData _biologicalData;

        [Inject]
        LiningAreaEnterBarrier _areaEnterBarrier;

        struct LivingAreaCollision : IJobProcessComponentData<LivingArea, InteractionElement>
        {
            [ReadOnly]
            public ComponentDataArray<Biological> Biological;
            //[ReadOnly]
            //public ComponentDataArray<BiologicalStatus> BiologicalStatus;
            [ReadOnly]
            public EntityCommandBuffer CommandBuffer;
            public EntityArchetype LivingInfoArchetype;
            
            public void Execute(ref LivingArea livingArea, ref InteractionElement interaction)
            {
                //for (int i = 0; i < BiologicalStatus.Length; i++)
                //{
                //    var status = BiologicalStatus[i];
                //    if (Vector3.Distance(status.Position, interaction.Position) <= interaction.Distance
                //        && status.TargetId == livingArea.Id
                //        && status.TargetType == interaction.Type
                //    )
                //    {
                //        LivingAreaEnterInfo info = new LivingAreaEnterInfo();
                //        EventInfo eventInfo = new EventInfo();
                //        eventInfo.Aid = Biological[i].BiologicalId;
                //        eventInfo.Bid = livingArea.Id;
                //        eventInfo.EventCode = 100;
                //        CommandBuffer.CreateEntity(LivingInfoArchetype);
                //        CommandBuffer.SetComponent(eventInfo);
                //        if (status)
                //            if (status.BiologicalIdentity == 0)
                //            {
                //                status.LocationType = LocationType.City;
                //                status.LocationId = livingArea.Id;
                //                //BiologicalStatus[i] = status;
                //            }
                //            else if (status.BiologicalIdentity == 1)
                //            {
                //                EventInfo eventInfo = new EventInfo();
                //                eventInfo.Aid = Biological[i].BiologicalId;
                //                eventInfo.Bid = livingArea.Id;
                //            }
                //    }
                //}
            }

            //public void Execute(int index)
            //{
            //    var status = BiologicalStatus[index];

            //    for (int i = 0; i < Interaction.Length; i++)
            //    {
            //        if (Vector3.Distance(BiologicalPosition[index].Value, Interaction[i].Position) <=Interaction[i].Distance 
            //            && status.TargetId==Interaction[i].Id
            //            &&status.TargetType==Interaction[i].Type)
            //        {

            //            status.TargetType = 0;
            //            BiologicalStatus[index] = status;
            //        }

            //    }

            //    //var biologicalIndex = Biological[index];

            //    //if (biologicalIndex.LocationType == LocationType.Field)
            //    //{
            //    //    float damage = 0.0f;
            //    //    float3 receiverPos = Positions[index].Value;
            //    //    var livingAreav = LivingAreas[0];
            //    //    for (int i = 0; i < LivingAreas.Length; i++)
            //    //    {
            //    //        float3 lpos = LivibfAreaPos[i].Value;
            //    //        float3 delta = lpos - receiverPos;
            //    //        float distSquared = math.dot(delta, delta);
            //    //        if (distSquared <= CollisionRadiusSquared)
            //    //        {
            //    //            livingAreav = LivingAreas[i];
            //    //            livingAreav.IsInternal = 1;
            //    //            LivingAreas[i] = livingAreav;
            //    //        }
            //    //    }
            //    //}
            //    //else if (biologicalIndex.LocationType == LocationType.None)
            //    //{
            //    //    float3 receiverPos = Positions[index].Value;
            //    //    for (int i = 0; i < LivingAreas.Length; i++)
            //    //    {
            //    //        float3 lpos = LivibfAreaPos[i].Value;
            //    //    }
            //    //}else if (biologicalIndex.LocationType == LocationType.LivingAreaIn)
            //    //{
            //    //    float3 receiverPos = Positions[index].Value;
            //    //}

            //    //var b = Biological[index];
            //       //b.LocationType = (int)LocationType.City;
            //       //b.LocationCode = livingAreav.Id;
            //       //Biological[index] = b;
            //       //MessageBoxInstance.Instance.MessageBoxShow("1");
            //       // UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);

            //        // MessageBoxInstance.Instance.MessageBoxShow();
            //        // livingAreav
            //        // LivingArea node = target.GetComponent<LivingArea>();
            //        // _livingAreasSelect.position = node.LivingAreaRender.bounds.center;
            //        // MessageBoxInstansce.Instance.MessageBoxShow("");

            //        //判断逻辑

            //        //if (CurPlayer != null)
            //        //{
            //        //    Debuger.Log("Enter LivingAreas");
            //        //    CurPlayer.transform.position = node.transform.position;
            //        //    //M_Strategy.InstanceLivingArea(node);

            //        //    ShowWindowData showWindowData = new ShowWindowData();
            //        //    showWindowData.contextData = new WindowContextLivingAreaNodeData(node);
            //        //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

            //        //    if (CurPlayer.GroupId == -1)
            //        //    {
            //        //        //  M_Strategy.EnterLivingAreas(node, CurPlayer);
            //        //    }
            //        //    else
            //        //    {
            //        //        //  M_Strategy.EnterLivingAreas(node, M_Biological.GroupsDic[CurPlayer.GroupId].Partners);
            //        //    }
            //        //}

            //        ///// 进入生活区
            //        ///// </summary>
            //        //public void LivingAreaEnter(LivingArea livingArea)
            //        //{
            //        //    if (livingArea == null)
            //        //    {
            //        //        Debuger.LogError("进入生活区时，数据为NULL");
            //        //        return;
            //        //    }


            //        //    //先对角色的属性进行检测 是否可以进入城市，1，角色位置是否在城市附近，2 角色与城市势力关系 3角色与城市首领关系 4城市的异常状态 
            //        //    //关闭当前开启的界面 ---
            //        //    UICenterMasterManager.Instance.CloseWindow(WindowID.LivingAreaBasicWindow);
            //        //    ShowWindowData showWindowData = new ShowWindowData();
            //        //    showWindowData.contextData = new WindowContextLivingAreaNodeData(livingArea);
            //        //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

            //        //    //    初始化LivingArea
            //        //    //    StaticValue.Instance.EnterLivingAreaId = livingArea.Id;


            //        //    //更新相机
            //        //    Renderer[] renderers = livingArea.LivingAreaM.GetComponentsInChildren<Renderer>();
            //        //    if (renderers.Length > 0)
            //        //    {
            //        //        Bounds mapBounds = renderers[0].bounds;
            //        //        for (int i = 0; i < renderers.Length; i++)
            //        //        {
            //        //            mapBounds.Encapsulate(renderers[i].bounds);
            //        //        }
            //        //        LivingfAreaCameraControl.SetBounds(mapBounds);
            //        //        LivingfAreaCameraControl.SetPosition(mapBounds.center);
            //        //    }
            //        //    //  M_LivingArea.EnterLivingArea(livingArea);
            //        //}
            //    //else
            //    //{
            //    //    return;
            //    //}
            //}
        }

        //[BurstCompile]
        //struct CollisionJob : IJobParallelFor
        //{
        //    public float CollisionRadiusSquared;

        //    public ComponentDataArray<Health> Health;
        //    [ReadOnly] public ComponentDataArray<Position> Positions;

        //    [NativeDisableParallelForRestriction]
        //    public ComponentDataArray<Shot> Shots;

        //    [NativeDisableParallelForRestriction] [ReadOnly]
        //    public ComponentDataArray<Position> ShotPositions;
        //    public void Execute(int index)
        //    {
        //        float damage = 0.0f;

        //        float3 receiverPos = Positions[index].Value;

        //        for (int si = 0; si < Shots.Length; ++si)
        //        {
        //            float3 shotPos = ShotPositions[si].Value;
        //            float3 delta = shotPos - receiverPos;
        //            float distSquared = math.dot(delta, delta);
        //            if (distSquared <= CollisionRadiusSquared)
        //            {
        //                var shot = Shots[si];

        //                damage += shot.Energy;

        //                // Set the shot's time to live to zero, so it will be collected by the shot destroy system
        //                shot.TimeToLive = 0.0f;

        //                Shots[si] = shot;
        //            }
        //        }

        //        var h = Health[index];
        //        h.Value = math.max(h.Value - damage, 0.0f);
        //        Health[index] = h;
        //    }
        //}



        //protected override JobHandle OnUpdate(JobHandle inputDeps)
        //{
        //    var palyersI = new LivingAreaCollision
        //    {
        //        LivingAreas = _livingAreaData.LivingArea,
        //        Interaction = _livingAreaData.Interaction,


        //    }.Schedule(_biologicalData.Length, 32, inputDeps);
        //    return palyersI;
        //}

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {


            var job = new LivingAreaCollision()
            {
                Biological = _biologicalData.Biological,
                // BiologicalStatus = _biologicalData.Status,
                CommandBuffer = _areaEnterBarrier.CreateCommandBuffer(),
                LivingInfoArchetype = GameSceneInit.EventInfotype
            };
            return job.Schedule(this, inputDeps);
        }

    }

}

