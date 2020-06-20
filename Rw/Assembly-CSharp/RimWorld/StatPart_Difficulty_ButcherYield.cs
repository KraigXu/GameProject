using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFC RID: 4092
	public class StatPart_Difficulty_ButcherYield : StatPart
	{
		// Token: 0x0600620D RID: 25101 RVA: 0x00220740 File Offset: 0x0021E940
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= Find.Storyteller.difficulty.butcherYieldFactor;
		}

		// Token: 0x0600620E RID: 25102 RVA: 0x00220758 File Offset: 0x0021E958
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficulty.butcherYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
