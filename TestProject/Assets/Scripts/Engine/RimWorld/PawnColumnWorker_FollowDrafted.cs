using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED0 RID: 3792
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x06005CFA RID: 23802 RVA: 0x0020425F File Offset: 0x0020245F
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06005CFB RID: 23803 RVA: 0x0020428D File Offset: 0x0020248D
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x06005CFC RID: 23804 RVA: 0x0020429A File Offset: 0x0020249A
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
