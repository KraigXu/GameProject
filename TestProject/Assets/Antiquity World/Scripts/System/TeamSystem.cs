using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


namespace GameSystem
{
    /// <summary>
    /// 队伍
    /// </summary>
    public class TeamSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Team> Teams;
        }

        [Inject]
        private Data _data;

        public static void SetupData(EntityManager entityManager,Entity entity,TeamData teamData)
        {
            entityManager.AddComponentData(entity, new Team()
            {
                TeamBossId = 0,
                Member = 6,
                Status = 1,
            });



        }

        public static void SetupComponentData(EntityManager entityManager)
        {

        }


        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var team = _data.Teams[i];
                var entity = _data.Entitys[i];
                //  var compoent = _data.TeamComs[i];

                team.Member= GameStaticData.TeamRunDic[entity].Members.Count;

                //TeamBaseInfo info;
                //if (GameStaticData.TeamRunDic.ContainsKey(entity) == false)
                //{
                //    info = new TeamBaseInfo();
                //   _teamBaseInfos.Add(compoent.Id, info);
                //}
                //else
                //{
                //    info = _teamBaseInfos[compoent.Id];
                //}

             //   info.Name = compoent.Name;
             //   info.Member = team.Member;
             //   info.Point = compoent.transform.position;
                //_teamBaseInfos[compoent.Id] = info;


                //var status = _data.Status[i];
                //var biological = _data.Biological[i];
                // if (team.TeamBossId == biological.BiologicalId)
                {
                    //switch (status.LocationType)
                    //{
                    //    case LocationType.None:
                    //        ModelStatus(team.RunModelCode, false);
                    //        break;
                    //    case LocationType.City:
                    //        if (status.TargetLocationType == LocationType.Field)
                    //        {
                    //            status.LocationType = status.TargetLocationType;
                    //        }
                    //        ModelStatus(team.RunModelCode, false);
                    //        break;
                    //    case LocationType.Field:
                    //        ModelStatus(team.RunModelCode, true);
                    //        ModelTarget(team.RunModelCode, status.TargetPosition);
                    //        break;
                    //}
                }
                //else
                {

                }

                //status.Position = ModelPosition(team.TeamBossId);

                //_data.Biological[i] = biological;
              //  _data.Status[i] = status;
            }
        }
    }

}
