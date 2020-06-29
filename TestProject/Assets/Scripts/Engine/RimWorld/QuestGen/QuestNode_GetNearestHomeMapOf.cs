using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetNearestHomeMapOf : QuestNode
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

		
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		
		public SlateRef<Thing> mapOf;
	}
}
