using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D0 RID: 2000
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x06003391 RID: 13201 RVA: 0x0011D9F0 File Offset: 0x0011BBF0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0011DA0A File Offset: 0x0011BC0A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}

		// Token: 0x04001BA8 RID: 7080
		private TrainableDef trainable;
	}
}
