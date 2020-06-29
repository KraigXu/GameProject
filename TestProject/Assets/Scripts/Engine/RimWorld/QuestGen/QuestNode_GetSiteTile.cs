using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetSiteTile : QuestNode
	{
		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<bool> preferCloserTiles;

		
		public SlateRef<bool> allowCaravans;
	}
}
