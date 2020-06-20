using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEC RID: 3308
	public class CompAssignableToPawn_Grave : CompAssignableToPawn
	{
		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x0600506B RID: 20587 RVA: 0x001B11B4 File Offset: 0x001AF3B4
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

		// Token: 0x0600506C RID: 20588 RVA: 0x001B1253 File Offset: 0x001AF453
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x001B1263 File Offset: 0x001AF463
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave((Building_Grave)this.parent);
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x001B127C File Offset: 0x001AF47C
		public override void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			pawn.ownership.UnclaimGrave();
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x001B128A File Offset: 0x001AF48A
		protected override bool ShouldShowAssignmentGizmo()
		{
			return !((Building_Grave)this.parent).HasCorpse;
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x001B129F File Offset: 0x001AF49F
		protected override string GetAssignmentGizmoLabel()
		{
			return "CommandGraveAssignColonistLabel".Translate();
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x001B12B0 File Offset: 0x001AF4B0
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandGraveAssignColonistDesc".Translate();
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001B12C4 File Offset: 0x001AF4C4
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
