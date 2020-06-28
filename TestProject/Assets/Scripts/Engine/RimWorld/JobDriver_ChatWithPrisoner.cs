using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000686 RID: 1670
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x000FA2DB File Offset: 0x000F84DB
		protected Pawn Talkee
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000FFE61 File Offset: 0x000FE061
		protected override string ReportStringProcessed(string str)
		{
			if (this.Talkee.guest.interactionMode == PrisonerInteractionModeDefOf.ReduceResistance)
			{
				return "JobReport_ReduceResistance".Translate(this.Talkee);
			}
			return base.ReportStringProcessed(str);
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000FFE9C File Offset: 0x000FE09C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Talkee.IsPrisonerOfColony || !this.Talkee.guest.PrisonerIsSecure);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !this.Talkee.guest.ScheduledForInteraction);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			yield break;
		}
	}
}
