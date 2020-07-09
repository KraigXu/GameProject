using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_MakeMinified : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MinifiedThing var = this.thing.GetValue(slate).MakeMinified();
			QuestGen.slate.Set<MinifiedThing>(this.storeAs.GetValue(slate), var, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Thing> thing;
	}
}
