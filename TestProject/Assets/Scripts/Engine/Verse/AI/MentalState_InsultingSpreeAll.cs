using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000547 RID: 1351
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		// Token: 0x0600269B RID: 9883 RVA: 0x000E37AE File Offset: 0x000E19AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000E37C8 File Offset: 0x000E19C8
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000E37D8 File Offset: 0x000E19D8
		public override void MentalStateTick()
		{
			if (this.target != null && !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, true))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(250) && (this.target == null || this.insultedTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000E3838 File Offset: 0x000E1A38
		private void ChooseNextTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_InsultingSpreeAll.candidates, true);
			if (!MentalState_InsultingSpreeAll.candidates.Any<Pawn>())
			{
				this.target = null;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
				return;
			}
			Pawn pawn;
			if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_InsultingSpreeAll.candidates.Any((Pawn x) => x != this.target))
			{
				pawn = (from x in MentalState_InsultingSpreeAll.candidates
				where x != this.target
				select x).RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			else
			{
				pawn = MentalState_InsultingSpreeAll.candidates.RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			if (pawn != this.target)
			{
				this.target = pawn;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000E3918 File Offset: 0x000E1B18
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return 1f - num + 0.01f;
		}

		// Token: 0x04001735 RID: 5941
		private int targetFoundTicks;

		// Token: 0x04001736 RID: 5942
		private const int CheckChooseNewTargetIntervalTicks = 250;

		// Token: 0x04001737 RID: 5943
		private const int MaxSameTargetChaseTicks = 1250;

		// Token: 0x04001738 RID: 5944
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
