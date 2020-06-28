﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x0010F64C File Offset: 0x0010D84C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x0011354C File Offset: 0x0011174C
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x0011358C File Offset: 0x0011178C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveSurgeryDoneNow;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x001135A0 File Offset: 0x001117A0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn || pawn2.InBed() || !pawn2.RaceProps.IsFlesh || !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2) || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.InMentalState && pawn2.MentalStateDef.IsAggro))
			{
				return false;
			}
			if (!pawn2.Downed)
			{
				if (pawn2.IsColonist)
				{
					return false;
				}
				if (!pawn2.IsPrisonerOfColony && pawn2.Faction != Faction.OfPlayer)
				{
					return false;
				}
				if (pawn2.guest != null && pawn2.guest.Released)
				{
					return false;
				}
			}
			Building_Bed building_Bed = base.FindBed(pawn, pawn2);
			return building_Bed != null && pawn2.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, -1, null, false);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x00113668 File Offset: 0x00111868
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Building_Bed t2 = base.FindBed(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.TakeToBedToOperate, pawn2, t2);
			job.count = 1;
			return job;
		}
	}
}
