using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074F RID: 1871
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		// Token: 0x06003103 RID: 12547 RVA: 0x001129CF File Offset: 0x00110BCF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Open))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x001129DF File Offset: 0x00110BDF
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Open);
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x001129F9 File Offset: 0x00110BF9
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x00112A2A File Offset: 0x00110C2A
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Open, t);
		}
	}
}
