using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000764 RID: 1892
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x001138A9 File Offset: 0x00111AA9
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			int num;
			for (int i = 0; i < desList.Count; i = num + 1)
			{
				if (desList[i].def == DesignationDefOf.Flick)
				{
					yield return desList[i].target.Thing;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x001138B9 File Offset: 0x00111AB9
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x001138D3 File Offset: 0x00111AD3
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x00113904 File Offset: 0x00111B04
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Flick, t);
		}
	}
}
