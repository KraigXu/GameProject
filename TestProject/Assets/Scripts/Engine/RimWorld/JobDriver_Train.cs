using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		
		
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{

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
