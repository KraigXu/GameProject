using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000665 RID: 1637
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000FE05E File Offset: 0x000FC25E
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000FE06B File Offset: 0x000FC26B
		public override string GetReport()
		{
			if (this.Takee == null || this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobUtility.GetResolvedJobReport(JobDefOf.Rescue.reportString, this.Takee);
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000FE0A9 File Offset: 0x000FC2A9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Takee == null || (!this.Takee.Downed && this.Takee.Awake()));
			foreach (Toil toil in this.<>n__0())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
