using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000630 RID: 1584
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000FB0B1 File Offset: 0x000F92B1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.BuildRoof[base.Cell]);
			this.FailOn(() => !RoofCollapseUtility.WithinRangeOfRoofHolder(base.Cell, base.Map, false));
			this.FailOn(() => !RoofCollapseUtility.ConnectedToRoofHolder(base.Cell, base.Map, true));
			foreach (Toil toil in this.<>n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000FB0C4 File Offset: 0x000F92C4
		protected override void DoEffect()
		{
			JobDriver_BuildRoof.builtRoofs.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map) && base.Map.areaManager.BuildRoof[intVec] && !intVec.Roofed(base.Map) && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map, false) && RoofUtility.FirstBlockingThing(intVec, base.Map) == null)
				{
					base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
					MoteMaker.PlaceTempRoof(intVec, base.Map);
					JobDriver_BuildRoof.builtRoofs.Add(intVec);
				}
			}
			JobDriver_BuildRoof.builtRoofs.Clear();
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000FB18D File Offset: 0x000F938D
		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}

		// Token: 0x0400199C RID: 6556
		private static List<IntVec3> builtRoofs = new List<IntVec3>();
	}
}
