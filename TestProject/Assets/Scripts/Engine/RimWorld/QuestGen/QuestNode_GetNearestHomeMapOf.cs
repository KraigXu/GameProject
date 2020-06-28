using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001132 RID: 4402
	public class QuestNode_GetNearestHomeMapOf : QuestNode
	{
		// Token: 0x060066E4 RID: 26340 RVA: 0x0024049B File Offset: 0x0023E69B
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x002404A5 File Offset: 0x0023E6A5
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x002404B4 File Offset: 0x0023E6B4
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				Map mapHeld = this.mapOf.GetValue(slate).MapHeld;
				if (mapHeld != null && mapHeld.IsPlayerHome)
				{
					slate.Set<Map>(this.storeAs.GetValue(slate), mapHeld, false);
					return;
				}
				int tile = this.mapOf.GetValue(slate).Tile;
				if (tile != -1)
				{
					Map map = null;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						if (maps[i].IsPlayerHome && (map == null || Find.WorldGrid.ApproxDistanceInTiles(tile, maps[i].Tile) < Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile)))
						{
							map = maps[i];
						}
					}
					if (map != null)
					{
						slate.Set<Map>(this.storeAs.GetValue(slate), map, false);
					}
				}
			}
		}

		// Token: 0x04003EFF RID: 16127
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x04003F00 RID: 16128
		public SlateRef<Thing> mapOf;
	}
}
