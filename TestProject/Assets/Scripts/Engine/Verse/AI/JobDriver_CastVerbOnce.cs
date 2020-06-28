using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000522 RID: 1314
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x0600257F RID: 9599 RVA: 0x000DE538 File Offset: 0x000DC738
		public override string GetReport()
		{
			string value;
			if (base.TargetA.HasThing)
			{
				value = base.TargetThingA.LabelCap;
			}
			else
			{
				value = "AreaLower".Translate();
			}
			return "UsingVerb".Translate(this.job.verbToUse.ReportLabel, value);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000DE59D File Offset: 0x000DC79D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
