using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDC RID: 3804
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x06005D3F RID: 23871 RVA: 0x002048BA File Offset: 0x00202ABA
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x002048CD File Offset: 0x00202ACD
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStage.LabelCap;
		}
	}
}
