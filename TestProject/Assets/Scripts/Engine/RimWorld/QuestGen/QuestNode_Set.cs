using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Set : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
		}

		
		[NoTranslate]
		public SlateRef<string> name;

		
		public SlateRef<object> value;
	}
}
