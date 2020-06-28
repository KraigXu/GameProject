using System;

namespace Verse
{
	// Token: 0x02000283 RID: 643
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06001131 RID: 4401 RVA: 0x00060EF1 File Offset: 0x0005F0F1
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x04000C5C RID: 3164
		private float chance = 1f;
	}
}
