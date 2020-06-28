using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200122D RID: 4653
	public static class CaravanBedUtility
	{
		// Token: 0x06006C59 RID: 27737 RVA: 0x0025C575 File Offset: 0x0025A775
		public static bool InCaravanBed(this Pawn p)
		{
			return p.CurrentCaravanBed() != null;
		}

		// Token: 0x06006C5A RID: 27738 RVA: 0x0025C580 File Offset: 0x0025A780
		public static Building_Bed CurrentCaravanBed(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			if (caravan == null)
			{
				return null;
			}
			return caravan.beds.GetBedUsedBy(p);
		}

		// Token: 0x06006C5B RID: 27739 RVA: 0x0025C5A5 File Offset: 0x0025A7A5
		public static bool WouldBenefitFromRestingInBed(Pawn p)
		{
			return !p.Dead && p.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x06006C5C RID: 27740 RVA: 0x0025C5C4 File Offset: 0x0025A7C4
		public static string AppendUsingBedsLabel(string str, int bedCount)
		{
			string str2 = (bedCount == 1) ? "UsingBedroll".Translate() : "UsingBedrolls".Translate(bedCount);
			return str + " (" + str2 + ")";
		}
	}
}
