using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001146 RID: 4422
	public class QuestNode_GetSiteTile : QuestNode
	{
		// Token: 0x06006739 RID: 26425 RVA: 0x00242058 File Offset: 0x00240258
		protected override bool TestRunInt(Slate slate)
		{
			int var;
			if (!this.TryFindTile(slate, out var))
			{
				return false;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x00242088 File Offset: 0x00240288
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int var;
			if (!this.TryFindTile(QuestGen.slate, out var))
			{
				return;
			}
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0600673B RID: 26427 RVA: 0x002420C4 File Offset: 0x002402C4
		private bool TryFindTile(Slate slate, out int tile)
		{
			Map map = slate.Get<Map>("map", null, false) ?? Find.RandomPlayerHomeMap;
			int nearThisTile = (map != null) ? map.Tile : -1;
			IntRange intRange;
			if (!slate.TryGet<IntRange>("siteDistRange", out intRange, false))
			{
				int minDist = 7;
				int maxDist = 27;
				bool value = this.preferCloserTiles.GetValue(slate);
				return TileFinder.TryFindNewSiteTile(out tile, minDist, maxDist, this.allowCaravans.GetValue(slate), value, nearThisTile);
			}
			return TileFinder.TryFindNewSiteTile(out tile, intRange.min, intRange.max, this.allowCaravans.GetValue(slate), this.preferCloserTiles.GetValue(slate), nearThisTile);
		}

		// Token: 0x04003F57 RID: 16215
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F58 RID: 16216
		public SlateRef<bool> preferCloserTiles;

		// Token: 0x04003F59 RID: 16217
		public SlateRef<bool> allowCaravans;
	}
}
