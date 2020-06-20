using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFB RID: 4091
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x06006209 RID: 25097 RVA: 0x00220687 File Offset: 0x0021E887
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x0600620A RID: 25098 RVA: 0x0022069E File Offset: 0x0021E89E
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x002206D4 File Offset: 0x0021E8D4
		private float Multiplier(DifficultyDef d)
		{
			int num = d.difficulty;
			if (num < 0 || num > this.factorsPerDifficulty.Count - 1)
			{
				Log.ErrorOnce("Not enough difficulty offsets defined for StatPart_Difficulty", 3598689, false);
				num = Mathf.Clamp(num, 0, this.factorsPerDifficulty.Count - 1);
			}
			return this.factorsPerDifficulty[num];
		}

		// Token: 0x04003BDA RID: 15322
		private List<float> factorsPerDifficulty = new List<float>();
	}
}
