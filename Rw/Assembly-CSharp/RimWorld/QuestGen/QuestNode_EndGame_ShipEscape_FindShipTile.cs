using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200119A RID: 4506
	public class QuestNode_EndGame_ShipEscape_FindShipTile : QuestNode
	{
		// Token: 0x06006857 RID: 26711 RVA: 0x002471F5 File Offset: 0x002453F5
		private bool TryFindRootTile(out int tile)
		{
			return TileFinder.TryFindRandomPlayerTile(out tile, false, delegate(int x)
			{
				int num;
				return this.TryFindDestinationTileActual(x, 180, out num);
			});
		}

		// Token: 0x06006858 RID: 26712 RVA: 0x0024720C File Offset: 0x0024540C
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

		// Token: 0x06006859 RID: 26713 RVA: 0x0024726C File Offset: 0x0024546C
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

		// Token: 0x0600685A RID: 26714 RVA: 0x002472C4 File Offset: 0x002454C4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int rootTile;
			this.TryFindRootTile(out rootTile);
			int var;
			this.TryFindDestinationTile(rootTile, out var);
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x00247300 File Offset: 0x00245500
		protected override bool TestRunInt(Slate slate)
		{
			int rootTile;
			int num;
			return this.TryFindRootTile(out rootTile) && this.TryFindDestinationTile(rootTile, out num);
		}

		// Token: 0x040040A7 RID: 16551
		private const int MinTraversalDistance = 180;

		// Token: 0x040040A8 RID: 16552
		private const int MaxTraversalDistance = 800;

		// Token: 0x040040A9 RID: 16553
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
