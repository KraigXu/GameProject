using TinyFrameWork;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace WX
{
    /// <summary>
    /// 场景交互
    /// </summary>
    class LivingAreaInteractionSystem : JobComponentSystem
    {
        struct Players
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Position> Position;
            public ComponentDataArray<Biological> Biological;
            [ReadOnly] public ComponentDataArray<PlayerInput> PlayerMarker;
        }
        [Inject]
        Players m_Players;

        struct LivingAreas
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<Position> Position;
            public ComponentDataArray<LivingArea> LivingArea;
        }
        [Inject]
        LivingAreas m_LivibgAreas;

        [BurstCompile]
        struct LivingAreaCollision : IJobParallelFor
        {
            public float CollisionRadiusSquared;
            public ComponentDataArray<Biological> Biological;
            [ReadOnly]
            public ComponentDataArray<Position> Positions;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<LivingArea> LivingAreas;

            [NativeDisableParallelForRestriction]
            [ReadOnly]
            public ComponentDataArray<Position> LivibfAreaPos;

            public void Execute(int index)
            {
                var biologicalIndex = Biological[index];
                if ((LocationType)biologicalIndex.LocationCode == LocationType.Field)
                {
                    float damage = 0.0f;
                    float3 receiverPos = Positions[index].Value;
                    var livingAreav= LivingAreas[0];
                    for (int i = 0; i < LivingAreas.Length; i++)
                    {
                        float3 lpos = LivibfAreaPos[i].Value;
                        float3 delta = lpos - receiverPos;
                        float distSquared = math.dot(delta, delta);
                        if (distSquared <= CollisionRadiusSquared)
                        {
                            livingAreav = LivingAreas[i];
                            livingAreav.IsInternal = 1;
                            LivingAreas[i] = livingAreav;
                        }
                    }

                    var b = Biological[index];
                    b.LocationType = (int)LocationType.City;
                    b.LocationCode = livingAreav.Id;
                    Biological[index] = b;
                    //MessageBoxInstance.Instance.MessageBoxShow("1");
                   // UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow);

                    //MessageBoxInstance.Instance.MessageBoxShow();
                    ////livingAreav
                    //LivingArea node = target.GetComponent<LivingArea>();
                    //  _livingAreasSelect.position = node.LivingAreaRender.bounds.center;
                    //  MessageBoxInstansce.Instance.MessageBoxShow("");

                    //判断逻辑

                    //if (CurPlayer != null)
                    //{
                    //    Debuger.Log("Enter LivingAreas");
                    //    CurPlayer.transform.position = node.transform.position;
                    //    //M_Strategy.InstanceLivingArea(node);

                    //    ShowWindowData showWindowData = new ShowWindowData();
                    //    showWindowData.contextData = new WindowContextLivingAreaNodeData(node);
                    //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

                    //    if (CurPlayer.GroupId == -1)
                    //    {
                    //        //  M_Strategy.EnterLivingAreas(node, CurPlayer);
                    //    }
                    //    else
                    //    {
                    //        //  M_Strategy.EnterLivingAreas(node, M_Biological.GroupsDic[CurPlayer.GroupId].Partners);
                    //    }
                    //}

                    ///// 进入生活区
                    ///// </summary>
                    //public void LivingAreaEnter(LivingArea livingArea)
                    //{
                    //    if (livingArea == null)
                    //    {
                    //        Debuger.LogError("进入生活区时，数据为NULL");
                    //        return;
                    //    }


                    //    //先对角色的属性进行检测 是否可以进入城市，1，角色位置是否在城市附近，2 角色与城市势力关系 3角色与城市首领关系 4城市的异常状态 


                    //    //关闭当前开启的界面 ---

                    //    UICenterMasterManager.Instance.CloseWindow(WindowID.LivingAreaBasicWindow);

                    //    ShowWindowData showWindowData = new ShowWindowData();
                    //    showWindowData.contextData = new WindowContextLivingAreaNodeData(livingArea);
                    //    UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, showWindowData);

                    //    //初始化LivingArea

                    //    //    StaticValue.Instance.EnterLivingAreaId = livingArea.Id;


                    //    //更新相机
                    //    Renderer[] renderers = livingArea.LivingAreaM.GetComponentsInChildren<Renderer>();
                    //    if (renderers.Length > 0)
                    //    {
                    //        Bounds mapBounds = renderers[0].bounds;
                    //        for (int i = 0; i < renderers.Length; i++)
                    //        {
                    //            mapBounds.Encapsulate(renderers[i].bounds);
                    //        }
                    //        LivingfAreaCameraControl.SetBounds(mapBounds);
                    //        LivingfAreaCameraControl.SetPosition(mapBounds.center);
                    //    }
                    //    //  M_LivingArea.EnterLivingArea(livingArea);
                    //}
                }
                else
                {
                    return;
                }
            }
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



        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            //var settings = TwoStickBootstrap.Settings;

            //if (settings == null)
            //    return inputDeps;

            var palyersI = new LivingAreaCollision
            {
                LivingAreas = m_LivibgAreas.LivingArea,
                LivibfAreaPos = m_LivibgAreas.Position,
                CollisionRadiusSquared = 1f,
                Biological = m_Players.Biological,
                Positions = m_Players.Position
            }.Schedule(m_Players.Length, 1, inputDeps);

            return palyersI;
            //var enemiesVsPlayers = new CollisionJob
            //{
            //    ShotPositions = m_EnemyShots.Position,
            //    Shots = m_EnemyShots.Shot,
            //    CollisionRadiusSquared = settings.playerCollisionRadius * settings.playerCollisionRadius,
            //    Health = m_Players.Health,
            //    Positions = m_Players.Position,
            //}.Schedule(m_Players.Length, 1, inputDeps);

            //var playersVsEnemies = new CollisionJob
            //{
            //    ShotPositions = m_PlayerShots.Position,
            //    Shots = m_PlayerShots.Shot,
            //    CollisionRadiusSquared = settings.enemyCollisionRadius * settings.enemyCollisionRadius,
            //    Health = m_Enemies.Health,
            //    Positions = m_Enemies.Position,
            //}.Schedule(m_Enemies.Length, 1, enemiesVsPlayers);

            //return playersVsEnemies;
        }
    }

}

