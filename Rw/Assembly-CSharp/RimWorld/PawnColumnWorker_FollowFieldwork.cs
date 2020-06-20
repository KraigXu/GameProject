using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED1 RID: 3793
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x06005CFE RID: 23806 RVA: 0x0020425F File Offset: 0x0020245F
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06005CFF RID: 23807 RVA: 0x002042A8 File Offset: 0x002024A8
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x06005D00 RID: 23808 RVA: 0x002042B5 File Offset: 0x002024B5
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
