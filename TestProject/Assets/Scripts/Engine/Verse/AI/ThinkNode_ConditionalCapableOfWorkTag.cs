using System;

namespace Verse.AI
{
	
	public class ThinkNode_ConditionalCapableOfWorkTag : ThinkNode_Conditional
	{
		
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCapableOfWorkTag thinkNode_ConditionalCapableOfWorkTag = (ThinkNode_ConditionalCapableOfWorkTag)base.DeepCopy(resolve);
			thinkNode_ConditionalCapableOfWorkTag.workTags = this.workTags;
			return thinkNode_ConditionalCapableOfWorkTag;
		}

		
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.WorkTagIsDisabled(this.workTags);
		}

		
		public WorkTags workTags;
	}
}
