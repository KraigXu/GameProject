using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_MultiplyRange : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<FloatRange>(this.storeAs.GetValue(slate), this.range.GetValue(slate) * this.value.GetValue(slate), false);
		}

		
		public SlateRef<FloatRange> range;

		
		public SlateRef<float> value;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
