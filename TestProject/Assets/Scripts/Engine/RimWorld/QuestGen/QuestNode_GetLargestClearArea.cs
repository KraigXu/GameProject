﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetLargestClearArea : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
			return largestSize >= this.failIfSmaller.GetValue(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int largestSize = this.GetLargestSize(slate);
			slate.Set<int>(this.storeAs.GetValue(slate), largestSize, false);
		}

		
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

		
		public SlateRef<Map> map;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<int> failIfSmaller;

		
		public SlateRef<int> max;
	}
}
