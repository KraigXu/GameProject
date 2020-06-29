using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class CompAssignableToPawn_Grave : CompAssignableToPawn
	{
		
		
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				IEnumerable<Pawn> second = from Corpse x in this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				where x.InnerPawn.IsColonist
				select x.InnerPawn;
				return this.parent.Map.mapPawns.FreeColonistsSpawned.Concat(second);
			}
		}

		
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave((Building_Grave)this.parent);
		}

		
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimGrave();
		}

		
		protected override bool ShouldShowAssignmentGizmo()
		{
			return !((Building_Grave)this.parent).HasCorpse;
		}

		
		protected override string GetAssignmentGizmoLabel()
		{
			return "CommandGraveAssignColonistLabel".Translate();
		}

		
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandGraveAssignColonistDesc".Translate();
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedGrave != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned grave. Removing.", false);
			}
		}
	}
}
