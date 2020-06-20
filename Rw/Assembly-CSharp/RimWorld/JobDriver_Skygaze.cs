using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200064D RID: 1613
	public class JobDriver_Skygaze : JobDriver
	{
		// Token: 0x06002C0C RID: 11276 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000FCA19 File Offset: 0x000FAC19
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

		// Token: 0x06002C0E RID: 11278 RVA: 0x000FCA2C File Offset: 0x000FAC2C
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
