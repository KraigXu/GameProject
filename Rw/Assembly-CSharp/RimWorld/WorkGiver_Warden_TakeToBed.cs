using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000738 RID: 1848
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		// Token: 0x06003073 RID: 12403 RVA: 0x0010FC98 File Offset: 0x0010DE98
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn prisoner = (Pawn)t;
			Job job = this.TakeDownedToBedJob(prisoner, pawn);
			if (job != null)
			{
				return job;
			}
			Job job2 = this.TakeToPreferredBedJob(prisoner, pawn);
			if (job2 != null)
			{
				return job2;
			}
			return null;
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x0010FCD8 File Offset: 0x0010DED8
		private Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			if (prisoner.Downed || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			if (RestUtility.FindBedFor(prisoner, prisoner, true, true, false) != null)
			{
				return null;
			}
			Room room = prisoner.GetRoom(RegionType.Set_Passable);
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, false, false);
			if (building_Bed != null && building_Bed.GetRoom(RegionType.Set_Passable) != room)
			{
				Job job = JobMaker.MakeJob(JobDefOf.EscortPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x0010FD50 File Offset: 0x0010DF50
		private Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Downed || !HealthAIUtility.ShouldSeekMedicalRestUrgent(prisoner) || prisoner.InBed() || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, true, false);
			if (building_Bed != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeWoundedPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}
	}
}
