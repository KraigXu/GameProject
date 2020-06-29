using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x00112DB0 File Offset: 0x00110FB0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x00112DB9 File Offset: 0x00110FB9
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x00112DC0 File Offset: 0x00110FC0
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
