using System;

namespace Verse.AI
{
	// Token: 0x0200058D RID: 1421
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x0600285E RID: 10334 RVA: 0x000EE9B7 File Offset: 0x000ECBB7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000EE9D1 File Offset: 0x000ECBD1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MentalStateDef == this.state;
		}

		// Token: 0x0400183F RID: 6207
		public MentalStateDef state;
	}
}
