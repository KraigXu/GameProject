using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000288 RID: 648
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x0600113C RID: 4412 RVA: 0x000613C8 File Offset: 0x0005F5C8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (this.minPlayerPopulation > 0 && pawn.Faction == Faction.OfPlayer && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.Count<Pawn>() < this.minPlayerPopulation)
				{
					return;
				}
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}

		// Token: 0x04000C64 RID: 3172
		public SimpleCurve ageFractionMtbDaysCurve;

		// Token: 0x04000C65 RID: 3173
		public int minPlayerPopulation;
	}
}
