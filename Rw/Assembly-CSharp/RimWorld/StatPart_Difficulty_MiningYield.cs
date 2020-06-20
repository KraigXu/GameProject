using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFD RID: 4093
	public class StatPart_Difficulty_MiningYield : StatPart
	{
		// Token: 0x06006210 RID: 25104 RVA: 0x002207AD File Offset: 0x0021E9AD
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= Find.Storyteller.difficulty.mineYieldFactor;
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x002207C4 File Offset: 0x0021E9C4
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficulty.mineYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
