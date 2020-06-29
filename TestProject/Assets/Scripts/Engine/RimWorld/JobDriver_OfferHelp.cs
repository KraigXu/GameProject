﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_OfferHelp : JobDriver
	{
		
		
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.OtherPawn.mindState.WillJoinColonyIfRescued);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.DoAtomic(delegate
			{
				this.OtherPawn.mindState.JoinColonyBecauseRescuedBy(this.pawn);
			});
			yield break;
		}

		
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
