using System;

namespace Verse.AI
{
	// Token: 0x02000594 RID: 1428
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x06002871 RID: 10353 RVA: 0x000EEB52 File Offset: 0x000ECD52
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000EEB6C File Offset: 0x000ECD6C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}

		// Token: 0x04001844 RID: 6212
		public Intelligence minIntelligence = Intelligence.ToolUser;
	}
}
