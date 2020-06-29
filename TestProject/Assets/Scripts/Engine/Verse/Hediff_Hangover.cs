using System;
using RimWorld;

namespace Verse
{
	
	public class Hediff_Hangover : HediffWithComps
	{
		
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
