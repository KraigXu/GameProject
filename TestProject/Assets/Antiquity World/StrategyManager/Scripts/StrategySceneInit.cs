using System;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using GameSystem.Ui;
using Manager;
using Newtonsoft.Json;
using AntiquityWorld.StrategyManager;
using Unity.Collections;


namespace GameSystem
{
    public sealed class StrategySceneInit
    {

        public static EntityArchetype DistrictArchetype;
        public static EntityArchetype TechniquesArchetype;
        public static EntityArchetype RelationArchetype;
        public static EntityArchetype BiologicalArchetype;
        public static EntityArchetype EventInfotype;
        public static EntityArchetype LivingAreaArchetype;
        public static EntityArchetype BiologicalSocialArchetype;
        public static EntityArchetype AncientTombArchetype;
        public static EntityArchetype ArticleArchetype;
        public static EntityArchetype FactionArchetype;

        public static DemoSetting Settings;

        public static void InitializeWithScene()
        {
          

            var settingsGo = GameObject.Find("Settings");
            if (settingsGo == null)
            {
                return;
            }
            else
            {
                Settings = settingsGo?.GetComponent<DemoSetting>();
                if (!Settings)
                {
                    return;
                }
            }
            UICenterMasterManager.Instance.ShowWindow(WindowID.LoadingWindow);
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
            DistrictArchetype = entityManager.CreateArchetype(typeof(District));
            LivingAreaArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(LivingArea));
            TechniquesArchetype = entityManager.CreateArchetype(typeof(Techniques));
            RelationArchetype = entityManager.CreateArchetype(typeof(Relation));
            BiologicalArchetype = entityManager.CreateArchetype(typeof(Element), typeof(Position), typeof(Rotation), typeof(Biological));
            EventInfotype = entityManager.CreateArchetype(typeof(EventInfo));
            BiologicalSocialArchetype = entityManager.CreateArchetype(typeof(BiologicalSocial));
            AncientTombArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Rotation));
            ArticleArchetype = entityManager.CreateArchetype(typeof(ArticleItem));
            FactionArchetype = entityManager.CreateArchetype(typeof(Faction));

            SystemManager.Get<ArticleSystem>().SetupComponentData(entityManager);
            SystemManager.Get<LivingAreaSystem>().SetupComponentData(entityManager);
            SystemManager.Get<DistrictSystem>().SetupComponentData(entityManager);
            SystemManager.Get<BiologicalSystem>().SetupComponentData(entityManager);

            #region Relation
            {
                List<RelationData> relationDatas = SQLService.Instance.QueryAll<RelationData>();
                for (int i = 0; i < relationDatas.Count; i++)
                {
                    Entity entity = entityManager.CreateEntity(RelationArchetype);
                    entityManager.SetComponentData(entity, new Relation
                    {
                        ObjectAid = relationDatas[i].ObjectAid,
                        ObjectBid = relationDatas[i].ObjectBid,
                        Value = relationDatas[i].Value
                    });
                }
            }
            #endregion

            #region TechniquesData
            {
                List<TechniquesData> techniquesDatas = SQLService.Instance.QueryAll<TechniquesData>();
                for (int i = 0; i < techniquesDatas.Count; i++)
                {
                    Entity techniques = entityManager.CreateEntity(TechniquesArchetype);
                    entityManager.SetComponentData(techniques, new Techniques
                    {
                        Id = techniquesDatas[i].Id,
                        ParentId = techniquesDatas[i].ParentId,
                    });

                    GameStaticData.TechniquesName.Add(techniquesDatas[i].Id, techniquesDatas[i].Name);
                    GameStaticData.TechniquesDescription.Add(techniquesDatas[i].Id, techniquesDatas[i].Description);
                    GameStaticData.TechniqueSprites.Add(techniquesDatas[i].Id, Resources.Load<Sprite>(techniquesDatas[i].AvatarPath));
                }
            }
            #endregion

            #region FactionData
            {

                List<FactionData> factionDatas = SQLService.Instance.QueryAll<FactionData>();

                for (int i = 0; i < factionDatas.Count; i++)
                {
                    Entity faction = entityManager.CreateEntity(FactionArchetype);
                    entityManager.SetComponentData(faction, new Faction
                    {
                        Id = factionDatas[i].Id,
                        Level = factionDatas[i].FactionLevel,
                        Type = factionDatas[i].FactionType,

                        Food = factionDatas[i].Food,
                        FoodMax = factionDatas[i].FoodMax,
                        Iron = factionDatas[i].Iron,
                        IronMax = factionDatas[i].IronMax,
                        Money = factionDatas[i].Money,
                        MoneyMax = factionDatas[i].MoneyMax,
                        Wood = factionDatas[i].Wood,
                        WoodMax = factionDatas[i].WoodMax
                    });

                    GameStaticData.FactionName.Add(factionDatas[i].Id, factionDatas[i].Name);
                    GameStaticData.FactionDescription.Add(factionDatas[i].Id, factionDatas[i].Description);
                }
            }
            #endregion

            #region FamilySystem
            {
                EntityArchetype familyArchetype = entityManager.CreateArchetype(typeof(Family));
                List<FamilyData> familyData = SQLService.Instance.QueryAll<FamilyData>();
                for (int i = 0; i < familyData.Count; i++)
                {
                    Entity family = entityManager.CreateEntity(familyArchetype);
                    entityManager.SetComponentData(family, new Family
                    {
                        FamilyId = familyData[i].Id
                    });
                }
            }

            #endregion


            RelationSystem.SetupComponentData(entityManager);
            SocialDialogSystem.SetupComponentData(entityManager);
            PrestigeSystem.SetupComponentData(entityManager);

            #region InitOver

            {
                //List<BiologicalData> data = SQLService.Instance.QueryAll<BiologicalData>();
                //for (int i = 0; i < data.Count; i++)
                //{
                //    if (data[i].Id == Settings.PlayerId)
                //    {
                //        StrategyCameraManager.Instance.SetTarget(new Vector3(data[i].X, data[i].Y, data[i].Z), true);
                //    }
                //}
                //data = null;

                UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);
                UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);

                UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);

                UICenterMasterManager.Instance.DestroyWindow(WindowID.LoadingWindow);

                StrategyCameraManager.Instance.SetTarget(new Vector3(-54.42019f, 50.3085f, 40.11046f));

            }

            #endregion
        }

        private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            UnityEngine.Object.Destroy(proto);
            return result;
        }

    }

}


