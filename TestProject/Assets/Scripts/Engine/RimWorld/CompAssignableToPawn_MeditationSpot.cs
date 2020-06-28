using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE9 RID: 3305
	public class CompAssignableToPawn_MeditationSpot : CompAssignableToPawn
	{
		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06005052 RID: 20562 RVA: 0x001B0D8E File Offset: 0x001AEF8E
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x001B0DCC File Offset: 0x001AEFCC
		public override string CompInspectStringExtra()
		{
			if (base.AssignedPawnsForReading.Count == 0)
			{
				return "Owner".Translate() + ": " + "Nobody".Translate();
			}
			if (base.AssignedPawnsForReading.Count == 1)
			{
				return "Owner".Translate() + ": " + base.AssignedPawnsForReading[0].Label;
			}
			return "";
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x001B0E52 File Offset: 0x001AF052
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedMeditationSpot != null;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x001B0E62 File Offset: 0x001AF062
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimMeditationSpot((Building)this.parent);
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x001B0E7B File Offset: 0x001AF07B
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimMeditationSpot();
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x001B0E8C File Offset: 0x001AF08C
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedMeditationSpot != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned meditation spot. Removing.", false);
			}
		}
	}
}
