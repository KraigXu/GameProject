using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072E RID: 1838
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x0600304A RID: 12362
		protected abstract DesignationDef Designation { get; }

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x0600304B RID: 12363
		protected abstract JobDef RemoveBuildingJob { get; }

		// Token: 0x0600304C RID: 12364 RVA: 0x0010F46B File Offset: 0x0010D66B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.Designation))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x0010F482 File Offset: 0x0010D682
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.Designation);
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x0010F49D File Offset: 0x0010D69D
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.CanReserve(t, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, this.Designation) != null;
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x0010F4CF File Offset: 0x0010D6CF
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.RemoveBuildingJob, t);
		}
	}
}
