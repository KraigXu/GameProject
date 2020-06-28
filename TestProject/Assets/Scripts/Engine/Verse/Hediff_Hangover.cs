using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000236 RID: 566
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0005B8F3 File Offset: 0x00059AF3
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
