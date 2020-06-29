using System;

namespace RimWorld
{
	
	public class CompProperties_AbilityTransferEntropy : CompProperties_AbilityEffect
	{
		
		public CompProperties_AbilityTransferEntropy()
		{
			this.compClass = typeof(CompAbilityEffect_TransferEntropy);
		}

		
		public bool targetReceivesEntropy = true;
	}
}
