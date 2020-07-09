using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetFactionOf : QuestNode
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
			Faction var = null;
			Thing value = this.thing.GetValue(slate);
			if (value != null)
			{
				var = value.Faction;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), var, false);
		}

		
		public SlateRef<Thing> thing;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
