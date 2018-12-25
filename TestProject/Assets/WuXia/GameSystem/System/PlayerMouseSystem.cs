using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace GameSystem
{
    public class PlayerMouseSystem : JobComponentSystem
    {
        struct Input
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> PlayerInput;
        }

        [Inject] private Input _input;

        struct Interaction
        {
            public readonly int Length;
            public ComponentDataArray<InteractionElement> Interactions;
        }
        [Inject] private Interaction _interaction;

        struct MouseOverInteraction: IJobParallelFor
        {
            public ComponentDataArray<PlayerInput> playerInput;

            [ReadOnly] public ComponentDataArray<InteractionElement> interaction;

            public void Execute(int index)
            {
                var input = playerInput[index];
                input.TouchedElement = ElementType.None;
                input.TouchedId = 0;
                for (int i = 0; i < interaction.Length; i++)
                {
                    if (Vector3.Distance(input.MousePoint, interaction[i].Position) <= interaction[i].Distance)
                    {
                        input.TouchedElement = interaction[i].Type;
                        input.TouchedId = interaction[i].Id;
                    }
                }
                playerInput[index] = input;

            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var mouveoverInaction=new MouseOverInteraction
            {
                playerInput=_input.PlayerInput,
                interaction = _interaction.Interactions
            };
            return mouveoverInaction.Schedule(_input.Length, 32, inputDeps);
        }
    }


}
