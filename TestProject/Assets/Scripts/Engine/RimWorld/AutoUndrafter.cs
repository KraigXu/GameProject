using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B76 RID: 2934
	public class AutoUndrafter : IExposable
	{
		// Token: 0x060044AB RID: 17579 RVA: 0x0017320D File Offset: 0x0017140D
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0017321C File Offset: 0x0017141C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x00173230 File Offset: 0x00171430
		public void AutoUndraftTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0 && this.pawn.Drafted)
			{
				if ((this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.def != JobDefOf.Wait_Combat) || this.AnyHostilePreventingAutoUndraft())
				{
					this.lastNonWaitingTick = Find.TickManager.TicksGame;
				}
				if (this.ShouldAutoUndraft())
				{
					this.pawn.drafter.Drafted = false;
				}
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x001732B8 File Offset: 0x001714B8
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x001732CA File Offset: 0x001714CA
		private bool ShouldAutoUndraft()
		{
			return Find.TickManager.TicksGame - this.lastNonWaitingTick >= 10000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x001732F4 File Offset: 0x001714F4
		private bool AnyHostilePreventingAutoUndraft()
		{
			List<IAttackTarget> potentialTargetsFor = this.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(this.pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (GenHostility.IsActiveThreatToPlayer(potentialTargetsFor[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002739 RID: 10041
		private Pawn pawn;

		// Token: 0x0400273A RID: 10042
		private int lastNonWaitingTick;

		// Token: 0x0400273B RID: 10043
		private const int UndraftDelay = 10000;
	}
}
