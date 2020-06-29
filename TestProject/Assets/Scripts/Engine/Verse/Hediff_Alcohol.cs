using System;
using RimWorld;

namespace Verse
{
	
	public class Hediff_Alcohol : Hediff_High
	{
		
		public override void Tick()
		{
			base.Tick();
			if (this.CurStageIndex >= 3 && this.pawn.IsHashIntervalTick(300) && this.HangoverSusceptible(this.pawn))
			{
				Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
				if (hediff != null)
				{
					hediff.Severity = 1f;
					return;
				}
				hediff = HediffMaker.MakeHediff(HediffDefOf.Hangover, this.pawn, null);
				hediff.Severity = 1f;
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		
		private const int HangoverCheckInterval = 300;
	}
}
