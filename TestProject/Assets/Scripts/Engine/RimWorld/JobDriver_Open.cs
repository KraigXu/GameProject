using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200066C RID: 1644
	public class JobDriver_Open : JobDriver
	{
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002CD4 RID: 11476 RVA: 0x000FE8E0 File Offset: 0x000FCAE0
		private IOpenable Openable
		{
			get
			{
				return (IOpenable)this.job.targetA.Thing;
			}
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000FE8F7 File Offset: 0x000FCAF7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = delegate
				{
					if (!this.Openable.CanOpen)
					{
						Designation designation = base.Map.designationManager.DesignationOn(this.job.targetA.Thing, DesignationDefOf.Open);
						if (designation != null)
						{
							designation.Delete();
						}
					}
				}
			}.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Open).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(300, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.A);
			yield break;
		}

		// Token: 0x040019FE RID: 6654
		public const int OpenTicks = 300;
	}
}
