using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081B RID: 2075
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		// Token: 0x06003443 RID: 13379 RVA: 0x0011F400 File Offset: 0x0011D600
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			Hediff firstHediffOfDef = other.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
			if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
			{
				return false;
			}
			return true;
		}
	}
}
