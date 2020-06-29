using System;
using System.Collections.Generic;

namespace Verse.AI
{
	
	public class JobDriver_CastVerbOnce : JobDriver
	{
		
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

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
