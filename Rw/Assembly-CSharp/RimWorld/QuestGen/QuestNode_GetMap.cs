using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200112A RID: 4394
	public class QuestNode_GetMap : QuestNode
	{
		// Token: 0x060066C0 RID: 26304 RVA: 0x0023F578 File Offset: 0x0023D778
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

		// Token: 0x060066C1 RID: 26305 RVA: 0x0023F5CC File Offset: 0x0023D7CC
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

		// Token: 0x060066C2 RID: 26306 RVA: 0x0023F628 File Offset: 0x0023D828
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

		// Token: 0x060066C3 RID: 26307 RVA: 0x0023F6C8 File Offset: 0x0023D8C8
		private bool IsAcceptableMap(Map map, Slate slate)
		{
			IntVec3 intVec;
			return map != null && (!this.mustBeInfestable.GetValue(slate) || InfestationCellFinder.TryFindCell(out intVec, map));
		}

		// Token: 0x04003EE4 RID: 16100
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x04003EE5 RID: 16101
		public SlateRef<bool> mustBeInfestable;

		// Token: 0x04003EE6 RID: 16102
		public SlateRef<int> preferMapWithMinFreeColonists;
	}
}
