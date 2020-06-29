using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		
		
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		
		public override string GetReport()
		{
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobUtility.GetResolvedJobReport(JobDefOf.Rescue.reportString, this.Takee);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Takee == null || (!this.Takee.Downed && this.Takee.Awake()));


			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
