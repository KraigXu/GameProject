using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000731 RID: 1841
	public abstract class WorkGiver_Warden : WorkGiver_Scanner
	{
		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x0600305E RID: 12382 RVA: 0x0010F64C File Offset: 0x0010D84C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x0001028D File Offset: 0x0000E48D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x0010F655 File Offset: 0x0010D855
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.PrisonersOfColonySpawned;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x0010F667 File Offset: 0x0010D867
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.PrisonersOfColonySpawnedCount == 0;
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x0010F67C File Offset: 0x0010D87C
		protected bool ShouldTakeCareOfPrisoner(Pawn warden, Thing prisoner)
		{
			Pawn pawn = prisoner as Pawn;
			return pawn != null && pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !prisoner.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, false);
		}
	}
}
