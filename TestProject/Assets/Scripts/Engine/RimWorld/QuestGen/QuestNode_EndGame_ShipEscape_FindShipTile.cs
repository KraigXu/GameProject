using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_EndGame_ShipEscape_FindShipTile : QuestNode
	{
		
		private bool TryFindRootTile(out int tile)
		{
			return TileFinder.TryFindRandomPlayerTile(out tile, false, delegate(int x)
			{
				int num;
				return this.TryFindDestinationTileActual(x, 180, out num);
			});
		}

		
		private bool TryFindDestinationTile(int rootTile, out int tile)
		{
			int num = 800;
			for (int i = 0; i < 1000; i++)
			{
				num = (int)((float)num * Rand.Range(0.5f, 0.75f));
				if (num <= 180)
				{
					num = 180;
				}
				if (this.TryFindDestinationTileActual(rootTile, num, out tile))
				{
					return true;
				}
				if (num <= 180)
				{
					return false;
				}
			}
			tile = -1;
			return false;
		}

		
		private bool TryFindDestinationTileActual(int rootTile, int minDist, out int tile)
		{
			for (int i = 0; i < 2; i++)
			{
				bool canTraverseImpassable = i == 1;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].biome.canBuildBase && Find.WorldGrid[x].biome.canAutoChoose, true, true, canTraverseImpassable))
				{
					return true;
				}
			}
			tile = -1;
			return false;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int rootTile;
			this.TryFindRootTile(out rootTile);
			int var;
			this.TryFindDestinationTile(rootTile, out var);
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		
		protected override bool TestRunInt(Slate slate)
		{
			int rootTile;
			int num;
			return this.TryFindRootTile(out rootTile) && this.TryFindDestinationTile(rootTile, out num);
		}

		
		private const int MinTraversalDistance = 180;

		
		private const int MaxTraversalDistance = 800;

		
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
