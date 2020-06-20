using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED3 RID: 3795
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x06005D07 RID: 23815 RVA: 0x001D243B File Offset: 0x001D063B
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x0020432D File Offset: 0x0020252D
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x0020433E File Offset: 0x0020253E
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x0020436F File Offset: 0x0020256F
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}
	}
}
