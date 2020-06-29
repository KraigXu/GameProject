using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000FA520 File Offset: 0x000F8720
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
			yield break;
			yield break;
		}

		
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryRecruit(TargetIndex.A);
		}
	}
}
