using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public class WorkGiver_Tend : WorkGiver_Scanner
	{
		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x0010FDBF File Offset: 0x0010DFBF
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x0010F64C File Offset: 0x0010D84C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x001136A2 File Offset: 0x001118A2
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWithAnyHediff;
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x001136B4 File Offset: 0x001118B4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && !pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && (!this.def.tendToHumanlikesOnly || pawn2.RaceProps.Humanlike) && (!this.def.tendToAnimalsOnly || pawn2.RaceProps.Animal) && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x00113732 File Offset: 0x00111932
		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			if (patient == doctor)
			{
				return true;
			}
			if (patient.RaceProps.Humanlike)
			{
				return patient.InBed();
			}
			return patient.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x00113758 File Offset: 0x00111958
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, pawn2);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2, thing);
			}
			return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2);
		}
	}
}
