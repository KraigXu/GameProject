using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		
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

		
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return 1f - num + 0.01f;
		}

		
		private int targetFoundTicks;

		
		private const int CheckChooseNewTargetIntervalTicks = 250;

		
		private const int MaxSameTargetChaseTicks = 1250;

		
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
