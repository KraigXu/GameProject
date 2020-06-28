using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200058F RID: 1423
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x06002864 RID: 10340 RVA: 0x000EEA2E File Offset: 0x000ECC2E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000EEA48 File Offset: 0x000ECC48
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x04001841 RID: 6209
		public List<MentalStateDef> states;
	}
}
