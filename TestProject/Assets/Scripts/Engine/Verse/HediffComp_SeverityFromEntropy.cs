using System;

namespace Verse
{
	
	public class HediffComp_SeverityFromEntropy : HediffComp
	{
		
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

		
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0005F020 File Offset: 0x0005D220
		public override bool CompShouldRemove
		{
			get
			{
				return this.EntropyAmount < float.Epsilon;
			}
		}

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.parent.Severity = this.EntropyAmount;
		}
	}
}
