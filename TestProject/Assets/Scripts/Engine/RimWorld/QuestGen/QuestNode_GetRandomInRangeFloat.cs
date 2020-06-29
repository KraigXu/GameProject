using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetRandomInRangeFloat : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<FloatRange> range;
	}
}
