using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000673 RID: 1651
	public class JobDriver_SingleInteraction : JobDriver
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x000FED24 File Offset: 0x000FCF24
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000FED4A File Offset: 0x000FCF4A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil toil = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			toil.socialMode = RandomSocialMode.Off;
			yield return toil;
			yield return Toils_Interpersonal.Interact(TargetIndex.A, this.job.interaction);
			yield break;
		}

		// Token: 0x04001A0A RID: 6666
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
