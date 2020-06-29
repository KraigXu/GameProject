using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x000FA582 File Offset: 0x000F8782
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Animal.training.NextTrainableToTrain() == null && !base.OnLastToil);
			yield break;
			yield break;
		}

		
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryTrain(TargetIndex.A);
		}
	}
}
