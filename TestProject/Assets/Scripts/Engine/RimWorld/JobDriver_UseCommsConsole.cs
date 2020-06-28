using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200067B RID: 1659
	public class JobDriver_UseCommsConsole : JobDriver
	{
		// Token: 0x06002D37 RID: 11575 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000FF8F5 File Offset: 0x000FDAF5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn((Toil to) => !((Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).CanUseCommsNow);
			Toil openComms = new Toil();
			openComms.initAction = delegate
			{
				Pawn actor = openComms.actor;
				if (((Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).CanUseCommsNow)
				{
					actor.jobs.curJob.commTarget.TryOpenComms(actor);
				}
			};
			yield return openComms;
			yield break;
		}
	}
}
