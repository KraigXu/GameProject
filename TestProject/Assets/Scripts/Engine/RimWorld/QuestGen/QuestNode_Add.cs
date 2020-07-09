using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_Add : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) + this.value2.GetValue(slate), false);
		}

		
		public SlateRef<double> value1;

		
		public SlateRef<double> value2;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
