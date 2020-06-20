using System;

namespace Verse
{
	// Token: 0x0200026D RID: 621
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x0005EFFB File Offset: 0x0005D1FB
		private float EntropyAmount
		{
			get
			{
				if (base.Pawn.psychicEntropy != null)
				{
					return base.Pawn.psychicEntropy.EntropyRelativeValue;
				}
				return 0f;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0005F020 File Offset: 0x0005D220
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0005F02F File Offset: 0x0005D22F
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}
