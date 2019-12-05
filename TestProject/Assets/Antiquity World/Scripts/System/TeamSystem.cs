using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
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
        public class Modeler
        {
            public GameObject Model;
            public int Id;
        }

        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Team> Teams;
        }

        [Inject] private Data _data;
        private int idCounter;
        private Transform _modelParent;

        public List<Modeler> ModelerArray = new List<Modeler>();

        private Dictionary<int,Entity> _dictionary=new Dictionary<int, Entity>();

      //  private Dictionary<int, TeamBaseInfo> _teamBaseInfos=new Dictionary<int, TeamBaseInfo>();


        //public Dictionary<int, TeamBaseInfo> TeamBaseInfos
        //{
        //    get { return _teamBaseInfos; }
        //}

        public static void SetupData(EntityManager entityManager,Entity entity,TeamData teamData)
        {
            entityManager.AddComponentData(entity, new Team()
            {
                TeamBossId = 0,
                Member = 6,
            });
        }


        public int AddModel(GameObject prefab, Vector3 point)
        {
            if (_modelParent == null)
            {
                GameObject modelparentgo=new GameObject("TeamModel");
                _modelParent = modelparentgo.transform;
            }
            GameObject go = GameObject.Instantiate(prefab, point, Quaternion.identity, _modelParent);
            Modeler modeler = new Modeler();
            modeler.Id = ++idCounter;
            modeler.Model = go;

            //modeler.Ai = go.GetComponent<AICharacterControl>();

            ModelerArray.Add(modeler);
            go.SetActive(false);
            return modeler.Id;
        }

        public int AddModel(GameObject prefab)
        {
            if (_modelParent == null)
            {
                GameObject modelparentgo = new GameObject("TeamModel");
                _modelParent = modelparentgo.transform;
            }

            GameObject go = GameObject.Instantiate(prefab,Vector3.zero , Quaternion.identity, _modelParent);
            Modeler modeler = new Modeler();
            modeler.Id = ++idCounter;
            modeler.Model = go;

          //  modeler.Ai = go.GetComponent<AICharacterControl>();

            ModelerArray.Add(modeler);
            go.SetActive(false);
            return modeler.Id;
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

        public List<Biological> TeamIdRetrunBiological(int biologicalId)
        {
            List<Biological> biologicals = new List<Biological>();

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Teams[i].TeamBossId == biologicalId)
                {
                  //  biologicals.Add(_data.Biological[i]);
                }
            }
            return biologicals;
        }

        public void ModelStatus(int id, bool flag)
        {
            for (int i = 0; i < ModelerArray.Count; i++)
            {
                if (ModelerArray[i].Id == id)
                {
                    if (ModelerArray[i].Model.activeSelf != flag)
                    {
                        ModelerArray[i].Model.SetActive(flag);
                    }
                    return;
                }
            }
        }

        public void ModelTarget(int id, Vector3 target)
        {
            for (int i = 0; i < ModelerArray.Count; i++)
            {
                if (ModelerArray[i].Id == id)
                {
                    //ModelerArray[i].Ai.SetTarget(target);
                    return;
                }
            }
        }

        public Vector3 ModelPosition(int id)
        {
            for (int i = 0; i < ModelerArray.Count; i++)
            {
                if (ModelerArray[i].Id == id)
                {
                    return ModelerArray[i].Model.transform.position;
                }
            }
            return Vector3.zero;
        }
    }

}
