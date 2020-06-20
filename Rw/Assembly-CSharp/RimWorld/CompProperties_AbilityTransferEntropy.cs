using System;

namespace RimWorld
{
	// Token: 0x02000AD9 RID: 2777
	public class CompProperties_AbilityTransferEntropy : CompProperties_AbilityEffect
	{
		// Token: 0x060041AB RID: 16811 RVA: 0x0015F1AE File Offset: 0x0015D3AE
		public CompProperties_AbilityTransferEntropy()
		{
			this.compClass = typeof(CompAbilityEffect_TransferEntropy);
		}

		// Token: 0x04002609 RID: 9737
		public bool targetReceivesEntropy = true;
	}
}
