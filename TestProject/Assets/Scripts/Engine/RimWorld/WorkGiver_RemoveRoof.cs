﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof.TrueCount == 0;
		}

		
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof[c] && c.Roofed(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced);
		}

		
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveRoof, c, c);
		}

		
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			IntVec3 cell = t.Cell;
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = cell + GenAdj.AdjacentCells[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.holdsRoof)
					{
						return -60f;
					}
					if (c.Roofed(pawn.Map))
					{
						num++;
					}
				}
			}
			return (float)(-(float)Mathf.Min(num, 3));
		}
	}
}
