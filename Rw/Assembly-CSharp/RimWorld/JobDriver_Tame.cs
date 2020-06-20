using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000626 RID: 1574
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000FA520 File Offset: 0x000F8720
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000FA530 File Offset: 0x000F8730
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<>n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
			yield break;
			yield break;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000FA540 File Offset: 0x000F8740
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryRecruit(TargetIndex.A);
		}
	}
}
