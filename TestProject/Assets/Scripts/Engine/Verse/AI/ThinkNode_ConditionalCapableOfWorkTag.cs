using System;

namespace Verse.AI
{
	// Token: 0x02000591 RID: 1425
	public class ThinkNode_ConditionalCapableOfWorkTag : ThinkNode_Conditional
	{
		// Token: 0x06002869 RID: 10345 RVA: 0x000EEA6B File Offset: 0x000ECC6B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCapableOfWorkTag thinkNode_ConditionalCapableOfWorkTag = (ThinkNode_ConditionalCapableOfWorkTag)base.DeepCopy(resolve);
			thinkNode_ConditionalCapableOfWorkTag.workTags = this.workTags;
			return thinkNode_ConditionalCapableOfWorkTag;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000EEA85 File Offset: 0x000ECC85
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(this.workTags);
		}

		// Token: 0x04001842 RID: 6210
		public WorkTags workTags;
	}
}
