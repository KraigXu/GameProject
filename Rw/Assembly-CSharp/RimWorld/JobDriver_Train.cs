using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000627 RID: 1575
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x000FA582 File Offset: 0x000F8782
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000FA592 File Offset: 0x000F8792
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<>n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Animal.training.NextTrainableToTrain() == null && !base.OnLastToil);
			yield break;
			yield break;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000FA5A2 File Offset: 0x000F87A2
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryTrain(TargetIndex.A);
		}
	}
}
