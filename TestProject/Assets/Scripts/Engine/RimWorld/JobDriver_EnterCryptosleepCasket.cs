﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_EnterCryptosleepCasket : JobDriver
	{
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = Toils_General.Wait(500, TargetIndex.None);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil;
			Toil enter = new Toil();
			enter.initAction = delegate
			{
				Pawn actor = enter.actor;
				Building_CryptosleepCasket pod = (Building_CryptosleepCasket)actor.CurJob.targetA.Thing;
				Action action = delegate
				{
					actor.DeSpawn(DestroyMode.Vanish);
					pod.TryAcceptThing(actor, true);
				};
				if (pod.def.building.isPlayerEjectable)
				{
					action();
					return;
				}
				if (this.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate(actor.Named("PAWN")).AdjustedFor(actor, "PAWN", true), action, false, null));
					return;
				}
				action();
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
			yield break;
		}
	}
}
