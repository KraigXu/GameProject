﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043D RID: 1085
	public static class GenClamor
	{
		// Token: 0x06002075 RID: 8309 RVA: 0x000C5F15 File Offset: 0x000C4115
		public static void DoClamor(Thing source, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, source.Position, radius, type);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000C5F28 File Offset: 0x000C4128
		public static void DoClamor(Thing source, IntVec3 position, float radius, ClamorDef type)
		{
			if (source.MapHeld == null)
			{
				return;
			}
			Region region = position.GetRegion(source.MapHeld, RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null || r.door.Open, delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					float num = Mathf.Clamp01(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing));
					if (num > 0f && pawn.Position.InHorDistOf(position, radius * num))
					{
						pawn.HearClamor(source, type);
					}
				}
				return false;
			}, 15, RegionType.Set_Passable);
		}
	}
}
