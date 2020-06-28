using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000235 RID: 565
	public class Hediff_Alcohol : Hediff_High
	{
		// Token: 0x06000FC7 RID: 4039 RVA: 0x0005B848 File Offset: 0x00059A48
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

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0001028D File Offset: 0x0000E48D
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04000BC8 RID: 3016
		private const int HangoverCheckInterval = 300;
	}
}
