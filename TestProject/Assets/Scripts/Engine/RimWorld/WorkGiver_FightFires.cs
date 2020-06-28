using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000740 RID: 1856
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x00111457 File Offset: 0x0010F657
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x00111464 File Offset: 0x0010F664
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Fire fire = t as Fire;
			if (fire == null)
			{
				return false;
			}
			Pawn pawn2 = fire.parent as Pawn;
			if (pawn2 != null)
			{
				if (pawn2 == pawn)
				{
					return false;
				}
				if ((pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction || pawn2.HostFaction == pawn.HostFaction) && !pawn.Map.areaManager.Home[fire.Position] && IntVec3Utility.ManhattanDistanceFlat(pawn.Position, pawn2.Position) > 15)
				{
					return false;
				}
				if (!pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			else
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Firefighting))
				{
					return false;
				}
				if (!pawn.Map.areaManager.Home[fire.Position])
				{
					JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
					return false;
				}
			}
			return ((pawn.Position - fire.Position).LengthHorizontalSquared <= 225 || pawn.CanReserve(fire, 1, -1, null, forced)) && !WorkGiver_FightFires.FireIsBeingHandled(fire, pawn);
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x0011157D File Offset: 0x0010F77D
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.BeatFire, t);
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x00111590 File Offset: 0x0010F790
		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			if (!f.Spawned)
			{
				return false;
			}
			Pawn pawn = f.Map.reservationManager.FirstRespectedReserver(f, potentialHandler);
			return pawn != null && pawn.Position.InHorDistOf(f.Position, 5f);
		}

		// Token: 0x04001AF3 RID: 6899
		private const int NearbyPawnRadius = 15;

		// Token: 0x04001AF4 RID: 6900
		private const int MaxReservationCheckDistance = 15;

		// Token: 0x04001AF5 RID: 6901
		private const float HandledDistance = 5f;
	}
}
