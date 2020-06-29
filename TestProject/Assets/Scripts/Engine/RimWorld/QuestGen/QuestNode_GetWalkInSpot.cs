using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetWalkInSpot : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return true;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 var;
			if (this.TryFindWalkInSpot(map, out var))
			{
				slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
				return true;
			}
			return false;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return;
			}
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			IntVec3 var;
			if (this.TryFindWalkInSpot(map, out var))
			{
				QuestGen.slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
			}
		}

		
		private bool TryFindWalkInSpot(Map map, out IntVec3 spawnSpot)
		{
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !c.Fogged(map) && map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return true;
			}
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return true;
			}
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => true, map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs = "walkInSpot";
	}
}
