using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200027F RID: 639
	public class HediffGiver_AddSeverity : HediffGiver
	{
		// Token: 0x06001122 RID: 4386 RVA: 0x00060B1C File Offset: 0x0005ED1C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (pawn.IsNestedHashIntervalTick(60, HediffGiver_AddSeverity.mtbCheckInterval) && Rand.MTBEventOccurs(this.mtbHours, 2500f, (float)HediffGiver_AddSeverity.mtbCheckInterval))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false).Severity += this.severityAmount;
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00060B8B File Offset: 0x0005ED8B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (float.IsNaN(this.severityAmount))
			{
				yield return "severityAmount is not defined";
			}
			if (this.mtbHours < 0f)
			{
				yield return "mtbHours is not defined";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000C53 RID: 3155
		public float severityAmount = float.NaN;

		// Token: 0x04000C54 RID: 3156
		public float mtbHours = -1f;

		// Token: 0x04000C55 RID: 3157
		private static int mtbCheckInterval = 1200;
	}
}
