using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001129 RID: 4393
	public class QuestNode_GetLargestClearArea : QuestNode
	{
		// Token: 0x060066BB RID: 26299 RVA: 0x0023F3BC File Offset: 0x0023D5BC
		protected override bool TestRunInt(Slate slate)
		{
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
			return largestSize >= this.failIfSmaller.GetValue(slate);
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x0023F3F8 File Offset: 0x0023D5F8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x0023F428 File Offset: 0x0023D628
		private int GetLargestSize(Slate slate)
		{
			Map mapResolved = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			if (mapResolved == null)
			{
				return 0;
			}
			int value = this.max.GetValue(slate);
			CellRect cellRect = LargestAreaFinder.FindLargestRect(mapResolved, (IntVec3 x) => this.IsClear(x, mapResolved), value);
			return Mathf.Min(new int[]
			{
				cellRect.Width,
				cellRect.Height,
				value
			});
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x0023F4B8 File Offset: 0x0023D6B8
		private bool IsClear(IntVec3 c, Map map)
		{
			if (!c.GetTerrain(map).affordances.Contains(TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial && thingList[i].Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (thingList[i].def.mineable)
				{
					bool flag = false;
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c2 = c + GenAdj.AdjacentCells[j];
						if (c2.InBounds(map) && c2.GetFirstMineable(map) == null)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04003EE0 RID: 16096
		public SlateRef<Map> map;

		// Token: 0x04003EE1 RID: 16097
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EE2 RID: 16098
		public SlateRef<int> failIfSmaller;

		// Token: 0x04003EE3 RID: 16099
		public SlateRef<int> max;
	}
}
