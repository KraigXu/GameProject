using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Skygaze : JobDriver
	{
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
			};
			toil.tickAction = delegate
			{
				float extraJoyGainFactor = this.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(this.pawn.Map);
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, null);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
			toil.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			yield return toil;
			yield break;
		}

		
		public override string GetReport()
		{
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				return "WatchingEclipse".Translate();
			}
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
			{
				return "WatchingAurora".Translate();
			}
			float num = GenCelestial.CurCelestialSunGlow(base.Map);
			if (num < 0.1f)
			{
				return "Stargazing".Translate();
			}
			if (num >= 0.65f)
			{
				return "CloudWatching".Translate();
			}
			if (GenLocalDate.DayPercent(this.pawn) < 0.5f)
			{
				return "WatchingSunrise".Translate();
			}
			return "WatchingSunset".Translate();
		}
	}
}
