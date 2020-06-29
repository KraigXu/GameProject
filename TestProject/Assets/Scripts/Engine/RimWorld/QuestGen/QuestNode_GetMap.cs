using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetMap : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			Map map;
			if (slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return true;
			}
			if (this.TryFindMap(slate, out map))
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
				return true;
			}
			return false;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map;
			if (QuestGen.slate.TryGet<Map>(this.storeAs.GetValue(slate), out map, false) && this.IsAcceptableMap(map, slate))
			{
				return;
			}
			if (this.TryFindMap(slate, out map))
			{
				QuestGen.slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
			}
		}

		
		private bool TryFindMap(Slate slate, out Map map)
		{
			int minCount;
			if (!this.preferMapWithMinFreeColonists.TryGetValue(slate, out minCount))
			{
				minCount = 1;
			}
			IEnumerable<Map> source = from x in Find.Maps
			where x.IsPlayerHome && this.IsAcceptableMap(x, slate)
			select x;
			if (!(from x in source
			where x.mapPawns.FreeColonists.Count >= minCount
			select x).TryRandomElement(out map))
			{
				return (from x in source
				where x.mapPawns.FreeColonists.Any<Pawn>()
				select x).TryRandomElement(out map);
			}
			return true;
		}

		
		private bool IsAcceptableMap(Map map, Slate slate)
		{
			IntVec3 intVec;
			return map != null && (!this.mustBeInfestable.GetValue(slate) || InfestationCellFinder.TryFindCell(out intVec, map));
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		
		public SlateRef<bool> mustBeInfestable;

		
		public SlateRef<int> preferMapWithMinFreeColonists;
	}
}
