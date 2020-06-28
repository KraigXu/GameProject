﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F9 RID: 1785
	public class JoyGiver_BuildSnowman : JoyGiver
	{
		// Token: 0x06002F41 RID: 12097 RVA: 0x001099D4 File Offset: 0x00107BD4
		public override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction))
			{
				return null;
			}
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null))
			{
				return null;
			}
			if (pawn.Map.snowGrid.TotalDepth < 200f)
			{
				return null;
			}
			IntVec3 c = JoyGiver_BuildSnowman.TryFindSnowmanBuildCell(pawn);
			if (!c.IsValid)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x00109A3C File Offset: 0x00107C3C
		private static IntVec3 TryFindSnowmanBuildCell(Pawn pawn)
		{
			Region rootReg;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Region r) => r.Room.PsychologicallyOutdoors, 100, out rootReg, RegionType.Set_Passable))
			{
				return IntVec3.Invalid;
			}
			IntVec3 result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.Room == rootReg.Room, delegate(Region r)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 randomCell = r.RandomCell;
					if (JoyGiver_BuildSnowman.IsGoodSnowmanCell(randomCell, pawn))
					{
						result = randomCell;
						return true;
					}
				}
				return false;
			}, 30, RegionType.Set_Passable);
			return result;
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x00109ADC File Offset: 0x00107CDC
		private static bool IsGoodSnowmanCell(IntVec3 c, Pawn pawn)
		{
			if (pawn.Map.snowGrid.GetDepth(c) < 0.5f)
			{
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			if (c.GetEdifice(pawn.Map) != null)
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (!c2.InBounds(pawn.Map))
				{
					return false;
				}
				if (!c2.Standable(pawn.Map))
				{
					return false;
				}
				if (pawn.Map.reservationManager.IsReservedAndRespected(c2, pawn))
				{
					return false;
				}
			}
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Snowman);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].Position.InHorDistOf(c, 12f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001AAE RID: 6830
		private const float MinSnowmanDepth = 0.5f;

		// Token: 0x04001AAF RID: 6831
		private const float MinDistBetweenSnowmen = 12f;
	}
}
