using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		
		
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
			yield break;
		}

		
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryRecruit(TargetIndex.A);
		}
	}
}
