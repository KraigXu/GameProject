using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetFreeColonistsCount : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
		private void SetVars(Slate slate)
		{
			int var;
			if (this.onlyThisMap.GetValue(slate) != null)
			{
				var = this.onlyThisMap.GetValue(slate).mapPawns.FreeColonistsCount;
			}
			else
			{
				var = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Map> onlyThisMap;
	}
}
