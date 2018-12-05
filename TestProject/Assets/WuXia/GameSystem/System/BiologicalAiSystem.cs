using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace  GameSystem
{
    /// <summary>
    /// 生物AI系统
    /// </summary>
    public class BiologicalAiSystem : ComponentSystem
    {

        struct BiologicalAiGroup
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<NpcInput> NpcInput;
            public ComponentArray<AICharacterControl> AiControl;
            public ComponentDataArray<BiologicalStatus> Status;
        }

        [Inject] private BiologicalAiGroup _aiGroup;

        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < _aiGroup.Length; i++)
            {
                Biological biological = _aiGroup.Biological[i];
                BiologicalStatus status = _aiGroup.Status[i];


                switch (status.TargetType)
                {
                    case ElementType.None: //如果对象没有Target类型 时 说明为闲置状态
                        if (status.LocationType == LocationType.Field)
                        {
                            //选择新目标
                            ChangeAiSystem(_aiGroup.Entitys[i], status, biological, _aiGroup.NpcInput[i]);
                        }
                        else
                        {

                        }

                        break;
                    case ElementType.Biological:
                        break;
                    case ElementType.District:
                        break;
                    case ElementType.LivingArea:
                        break;
                    case ElementType.Terrain:
                        break;
                }

                _aiGroup.Biological[i] = biological;
                _aiGroup.Status[i] = status;

                //switch ((LocationType)m_Players.Status[i].LocationType)
                //{
                //    case LocationType.None:
                //        break;
                //    case LocationType.Field:
                //        {
                //            newtarget.Target = m_Players.AiControl[i].transform.position;
                //        }
                //        break;
                //    case LocationType.LivingAreaEnter:
                //        {
                //            //检查当前状态 显示UI信息 

                //            ShowWindowData windowData = new ShowWindowData();
                //            LivingAreaWindowCD uidata = _livingAreaSystem.GetLivingAreaData(m_Players.Status[i].TargetId);

                //            uidata.OnOpen = LivingAreaOnOpen;
                //            uidata.OnExit = LivingAreaOnExit;
                //            windowData.contextData = uidata;
                //            UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaMainWindow, windowData);

                //            newStatus.LocationType = (int)LocationType.LivingAreaIn;
                //            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(GameStaticData.LivingAreaModelPath[m_Players.Status[i].TargetId]));
                //            Renderer[] renderers = go.transform.GetComponentsInChildren<Renderer>();

                //            Bounds bounds = renderers[0].bounds;

                //            for (int j = 1; j < renderers.Length; j++)
                //            {
                //                bounds.Encapsulate(renderers[j].bounds);
                //            }
                //            newtarget.Target = bounds.center;
                //        }
                //        break;
                //    case LocationType.LivingAreaIn:
                //        {

                //        }
                //        break;
                //    case LocationType.LivingAreaExit:
                //        {

                //        }
                //        break;
                //    case LocationType.SocialDialog:
                //        {
                //            Debug.Log("LLsl");
                //        }
                //        break;
                //}


                //switch ((TendType)_aiGroup.NpcInput[i].Movetend)
                //{
                //    case TendType.Money:
                //        break;
                //    case TendType.Move:
                //        if (_aiGroup.AiControl[i].IsMove == false)
                //        {
                //            _aiGroup.AiControl[i].SetTarget(new Vector3(Random.Range(1500f, 1700f), 80.7618f, Random.Range(500f, 700f)));
                //        }
                //        break;
                //    default:
                //        break;
                //}
            }       
        }


        /// <summary>
        /// 根据属性 选择合适的行为
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="status"></param>
        /// <param name="biological"></param>
        private void ChangeAiSystem(Entity entity, BiologicalStatus status, Biological biological, NpcInput input)
        {
            
            switch (input.BehaviorPolicy)
            {
                case BehaviorPolicyType.Cruising:
                    


                    break;
            }

        }

        private void ChangeAiSystem(Entity entity, BiologicalStatus status, Biological biological, NpcInput input,
            AICharacterControl aiCharacter)
        {

        }


        private void CheckAi()
        {

        }
    }
}

