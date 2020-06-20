using System;

namespace Verse.AI
{
	// Token: 0x0200058E RID: 1422
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x000EE9E9 File Offset: 0x000ECBE9
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000EEA04 File Offset: 0x000ECC04
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}

		// Token: 0x04001840 RID: 6208
		public Type stateClass;
	}
}
