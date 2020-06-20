using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public class CompAssignableToPawn_Bed : CompAssignableToPawn
	{
		// Token: 0x06005064 RID: 20580 RVA: 0x001B10C8 File Offset: 0x001AF2C8
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001B10D8 File Offset: 0x001AF2D8
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimBedIfNonMedical((Building_Bed)this.parent);
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001B10F1 File Offset: 0x001AF2F1
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimBed();
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001B1100 File Offset: 0x001AF300
		protected override bool ShouldShowAssignmentGizmo()
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			return building_Bed.def.building.bed_humanlike && building_Bed.Faction == Faction.OfPlayer && !building_Bed.ForPrisoners && !building_Bed.Medical;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001B114C File Offset: 0x001AF34C
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
