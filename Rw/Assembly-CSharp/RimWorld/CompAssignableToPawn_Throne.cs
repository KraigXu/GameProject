using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEA RID: 3306
	public class CompAssignableToPawn_Throne : CompAssignableToPawn
	{
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x0600505B RID: 20571 RVA: 0x001B0F18 File Offset: 0x001AF118
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				where p.royalty != null && p.royalty.AllTitlesForReading.Any<RoyalTitle>()
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x001B0F84 File Offset: 0x001AF184
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

		// Token: 0x0600505D RID: 20573 RVA: 0x001B100A File Offset: 0x001AF20A
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedThrone != null;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001B101A File Offset: 0x001AF21A
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimThrone((Building_Throne)this.parent);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001B1033 File Offset: 0x001AF233
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimThrone();
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001B1044 File Offset: 0x001AF244
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedThrone != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned throne. Removing.", false);
			}
		}
	}
}
