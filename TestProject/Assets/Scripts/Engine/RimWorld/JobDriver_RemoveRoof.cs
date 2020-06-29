﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		
		
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.NoRoof[base.Cell]);


			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true, false);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
