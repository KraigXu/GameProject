using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071C RID: 1820
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002FEE RID: 12270
		protected abstract JobDef JobDef { get; }

		// Token: 0x06002FEF RID: 12271
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x06002FF0 RID: 12272 RVA: 0x0010E07D File Offset: 0x0010C27D
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x0010E098 File Offset: 0x0010C298
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].RaceProps.Animal)
				{
					CompHasGatherableBodyResource comp = this.GetComp(list[i]);
					if (comp != null && comp.ActiveAndFull)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x0010E0FC File Offset: 0x0010C2FC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			CompHasGatherableBodyResource comp = this.GetComp(pawn2);
			return comp != null && comp.ActiveAndFull && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x0010E15B File Offset: 0x0010C35B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.JobDef, t);
		}
	}
}
