using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetMapOf : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), this.mapOf.GetValue(slate).MapHeld, false);
			}
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		
		public SlateRef<Thing> mapOf;
	}
}
