using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000FE05E File Offset: 0x000FC25E
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
			foreach (Toil toil in this.n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
