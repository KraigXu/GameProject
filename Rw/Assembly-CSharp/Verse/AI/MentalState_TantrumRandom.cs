using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200055E RID: 1374
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x0600270F RID: 9999
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06002710 RID: 10000 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000E4C17 File Offset: 0x000E2E17
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000E4C31 File Offset: 0x000E2E31
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000E4C40 File Offset: 0x000E2E40
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (this.target is Pawn && ((Pawn)this.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(500) && (this.target == null || this.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000E4CD4 File Offset: 0x000E2ED4
		private void ChooseNextTarget()
		{
			MentalState_TantrumRandom.candidates.Clear();
			this.GetPotentialTargets(MentalState_TantrumRandom.candidates);
			if (!MentalState_TantrumRandom.candidates.Any<Thing>())
			{
				this.target = null;
				this.hitTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Thing thing;
				if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_TantrumRandom.candidates.Any((Thing x) => x != this.target))
				{
					thing = (from x in MentalState_TantrumRandom.candidates
					where x != this.target
					select x).RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				else
				{
					thing = MentalState_TantrumRandom.candidates.RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				if (thing != this.target)
				{
					this.target = thing;
					this.hitTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
			MentalState_TantrumRandom.candidates.Clear();
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000E4DC8 File Offset: 0x000E2FC8
		private float GetCandidateWeight(Thing candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return (1f - num) * (1f - num) + 0.01f;
		}

		// Token: 0x04001754 RID: 5972
		private int targetFoundTicks;

		// Token: 0x04001755 RID: 5973
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x04001756 RID: 5974
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04001757 RID: 5975
		private static List<Thing> candidates = new List<Thing>();
	}
}
