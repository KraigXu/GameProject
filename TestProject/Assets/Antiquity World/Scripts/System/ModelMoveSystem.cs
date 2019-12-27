using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

namespace GameSystem
{
    public class ModelMoveSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<ModelComponent> Modeldata;
            public ComponentDataArray<BehaviorData> BData;
            public ComponentDataArray<Position> Position;
            public ComponentArray<NavMeshAgent> Agent;

        }

        [Inject]
        private Data _data;

        public List<Modeler> ModelerArray = new List<Modeler>();
        public class Modeler
        {
            public GameObject Model;
            public int Id;
        }

        private Transform _modelParent;
        private int idCounter;

        public static void SetupData(EntityManager entityManager, Entity entity, TeamData teamData)
        {

            entityManager.AddComponentData(entity,new BehaviorData()
            {

            });

            entityManager.AddComponentData(entity,new Position()
            {

            });

            entityManager.AddComponentData(entity, new ModelComponent()
            {
                Id = 1,
                MoveSpeed = 10,
                Speed = 10,
                Target = Vector3.down

            });
        }

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var move = _data.Modeldata[i];
                var behavior = _data.BData[i];
                var agent = _data.Agent[i];
                if (IsContains(move.Id))
                {
                    if (move.MoveSpeed == 1)
                    {
                        ModelStatus(move.Id,false);
                    }
                    else
                    {
                        ModelStatus(move.Id,true);
                        var position = new Position { Value = ModelTarget(move.Id, behavior.Target, move.Speed) };
                        _data.Position[i] = position;
                        agent.destination = position.Value;
                    }
                }
               
            }
        }

        public int AddModel(GameObject prefab, Vector3 point)
        {
            if (_modelParent == null)
            {
                GameObject modelparentgo = new GameObject("TeamModel");
                _modelParent = modelparentgo.transform;
            }

            GameObject go = GameObject.Instantiate(prefab, point, Quaternion.identity, _modelParent);
            Modeler modeler = new Modeler();
            modeler.Id = ++idCounter;
            modeler.Model = go;

          //  modeler.Ai = go.GetComponent<AICharacterControl>();

            ModelerArray.Add(modeler);
            go.SetActive(true);
            return modeler.Id;
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

        public bool IsContains(int id)
        {
            for (int i = 0; i < ModelerArray.Count; i++)
            {
                if (ModelerArray[i].Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector3 ModelTarget(int id, Vector3 target,float speed)
        {
            for (int i = 0; i < ModelerArray.Count; i++)
            {
                if (ModelerArray[i].Id == id)
                {
                   // ModelerArray[i].Ai.SetTarget(target);
                  //  return ModelerArray[i].Ai.transform.position;
                }
            }
            return Vector3.zero;
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
