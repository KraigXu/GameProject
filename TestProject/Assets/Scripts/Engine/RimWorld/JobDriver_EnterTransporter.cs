using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200065A RID: 1626
	public class JobDriver_EnterTransporter : JobDriver
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x000FD554 File Offset: 0x000FB754
		public CompTransporter Transporter
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000FD586 File Offset: 0x000FB786
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn(() => !this.Transporter.LoadingInProgressOrReadyToLaunch);
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate
				{
					CompTransporter transporter = this.Transporter;
					this.pawn.DeSpawn(DestroyMode.Vanish);
					transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
				}
			};
			yield break;
		}

		// Token: 0x040019D8 RID: 6616
		private TargetIndex TransporterInd = TargetIndex.A;
	}
}
