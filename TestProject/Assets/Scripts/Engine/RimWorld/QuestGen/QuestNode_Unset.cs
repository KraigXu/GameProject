using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_Unset : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			slate.Remove(this.name.GetValue(slate), false);
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Remove(this.name.GetValue(slate), false);
		}

		
		[NoTranslate]
		public SlateRef<string> name;
	}
}
