using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000766 RID: 1894
	public class WorkGiver_VisitSickPawn : WorkGiver_Scanner
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x0010FDBF File Offset: 0x0010DFBF
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x0010F64C File Offset: 0x0010D84C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x0011398B File Offset: 0x00111B8B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001139A4 File Offset: 0x00111BA4
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			if (!InteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return true;
			}
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].InBed())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001139F4 File Offset: 0x00111BF4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && SickPawnVisitUtility.CanVisit(pawn, pawn2, JoyCategory.VeryLow);
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x00113A18 File Offset: 0x00111C18
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Job job = JobMaker.MakeJob(JobDefOf.VisitSickPawn, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
