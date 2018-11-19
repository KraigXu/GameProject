using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace  WX
{

    public class BiologicalAiSystem : ComponentSystem
    {
        struct BiologicalAiGroup
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<NpcInput> NpcInput;
            public ComponentArray<AICharacterControl> AiControl;
        }

        [Inject] private BiologicalAiGroup _aiGroup;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _aiGroup.Length; i++)
            {
                switch ((TendType)_aiGroup.NpcInput[i].Movetend)
                {
                    case TendType.Money:
                       break;
                    case TendType.Move:
                        if (_aiGroup.AiControl[i].IsMove == false)
                        {
                            _aiGroup.AiControl[i].SetTarget(new Vector3(Random.Range(1500f,1700f),80.7618f,Random.Range(500f,700f)), (int)TragetType.Field, -1);
                        }
                        break;
                    default:
                        break;
                }
                
               
            }




        }



    }
}

