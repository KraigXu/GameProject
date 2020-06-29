using System;
using Verse;

namespace RimWorld
{
	
	public class CompAssignableToPawn_Bed : CompAssignableToPawn
	{
		
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimBedIfNonMedical((Building_Bed)this.parent);
		}

		
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimBed();
		}

		
		protected override bool ShouldShowAssignmentGizmo()
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			return building_Bed.def.building.bed_humanlike && building_Bed.Faction == Faction.OfPlayer && !building_Bed.ForPrisoners && !building_Bed.Medical;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.OwnedBed != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.", false);
			}
		}
	}
}
