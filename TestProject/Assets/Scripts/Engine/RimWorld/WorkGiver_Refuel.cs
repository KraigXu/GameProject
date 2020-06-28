using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000755 RID: 1877
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x00112DB0 File Offset: 0x00110FB0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x00112DB9 File Offset: 0x00110FB9
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x00112DC0 File Offset: 0x00110FC0
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x00112DC7 File Offset: 0x00110FC7
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x00112DD5 File Offset: 0x00110FD5
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x00112DEA File Offset: 0x00110FEA
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
