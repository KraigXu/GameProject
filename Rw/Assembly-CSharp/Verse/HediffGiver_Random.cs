using System;

namespace Verse
{
	// Token: 0x02000286 RID: 646
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x06001138 RID: 4408 RVA: 0x00061360 File Offset: 0x0005F560
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x04000C61 RID: 3169
		public float mtbDays;
	}
}
