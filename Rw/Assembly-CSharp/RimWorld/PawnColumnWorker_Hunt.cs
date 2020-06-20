using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED2 RID: 3794
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x06005D02 RID: 23810 RVA: 0x001D198E File Offset: 0x001CFB8E
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x002042C3 File Offset: 0x002024C3
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x06005D04 RID: 23812 RVA: 0x002042D4 File Offset: 0x002024D4
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x0020430D File Offset: 0x0020250D
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
		}
	}
}
