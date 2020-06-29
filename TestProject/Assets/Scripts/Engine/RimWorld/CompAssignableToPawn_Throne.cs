using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class CompAssignableToPawn_Throne : CompAssignableToPawn
	{
		
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

		
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedThrone != null;
		}

		
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimThrone((Building_Throne)this.parent);
		}

		
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimThrone();
		}

		
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
