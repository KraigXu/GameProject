using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED4 RID: 3796
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x06005D0C RID: 23820 RVA: 0x001D2BD1 File Offset: 0x001D0DD1
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x00204377 File Offset: 0x00202577
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x002042D4 File Offset: 0x002024D4
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06005D0F RID: 23823 RVA: 0x00204388 File Offset: 0x00202588
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn, false);
		}
	}
}
