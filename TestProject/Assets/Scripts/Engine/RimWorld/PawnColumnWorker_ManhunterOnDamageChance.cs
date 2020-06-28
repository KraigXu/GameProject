using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF0 RID: 3824
	public class PawnColumnWorker_ManhunterOnDamageChance : PawnColumnWorker_Text
	{
		// Token: 0x06005DC9 RID: 24009 RVA: 0x002065D1 File Offset: 0x002047D1
		protected override string GetTextFor(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnDamageChance(pawn, null).ToStringPercent();
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x002065DF File Offset: 0x002047DF
		protected override string GetTip(Pawn pawn)
		{
			return "HarmedRevengeChanceExplanation".Translate();
		}
	}
}
