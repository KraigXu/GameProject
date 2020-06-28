using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000672 RID: 1650
	public class JobDriver_Resurrect : JobDriver
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x000FEC1C File Offset: 0x000FCE1C
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x000FEC44 File Offset: 0x000FCE44
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000FEC68 File Offset: 0x000FCE68
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000FECB9 File Offset: 0x000FCEB9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(600, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			yield return Toils_General.Do(new Action(this.Resurrect));
			yield break;
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000FECCC File Offset: 0x000FCECC
		private void Resurrect()
		{
			Pawn innerPawn = this.Corpse.InnerPawn;
			ResurrectionUtility.ResurrectWithSideEffects(innerPawn);
			Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04001A07 RID: 6663
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04001A08 RID: 6664
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x04001A09 RID: 6665
		private const int DurationTicks = 600;
	}
}
