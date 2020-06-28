using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4A RID: 3914
	public static class TwelfthUtility
	{
		// Token: 0x06006046 RID: 24646 RVA: 0x00216990 File Offset: 0x00214B90
		public static Quadrum GetQuadrum(this Twelfth twelfth)
		{
			switch (twelfth)
			{
			case Twelfth.First:
				return Quadrum.Aprimay;
			case Twelfth.Second:
				return Quadrum.Aprimay;
			case Twelfth.Third:
				return Quadrum.Aprimay;
			case Twelfth.Fourth:
				return Quadrum.Jugust;
			case Twelfth.Fifth:
				return Quadrum.Jugust;
			case Twelfth.Sixth:
				return Quadrum.Jugust;
			case Twelfth.Seventh:
				return Quadrum.Septober;
			case Twelfth.Eighth:
				return Quadrum.Septober;
			case Twelfth.Ninth:
				return Quadrum.Septober;
			case Twelfth.Tenth:
				return Quadrum.Decembary;
			case Twelfth.Eleventh:
				return Quadrum.Decembary;
			case Twelfth.Twelfth:
				return Quadrum.Decembary;
			default:
				return Quadrum.Undefined;
			}
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x002169F0 File Offset: 0x00214BF0
		public static Twelfth PreviousTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			int num = (int)(twelfth - Twelfth.Second);
			if (num == -1)
			{
				num = 11;
			}
			return (Twelfth)num;
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x00216A12 File Offset: 0x00214C12
		public static Twelfth NextTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			return (twelfth + 1) % Twelfth.Undefined;
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x00216A23 File Offset: 0x00214C23
		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x00216A33 File Offset: 0x00214C33
		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x00216A40 File Offset: 0x00214C40
		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 16f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x00216A7C File Offset: 0x00214C7C
		public static Twelfth GetLeftMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			if (twelfths.Count >= 12)
			{
				return Twelfth.Undefined;
			}
			Twelfth result;
			do
			{
				result = rootTwelfth;
				rootTwelfth = TwelfthUtility.TwelfthBefore(rootTwelfth);
			}
			while (twelfths.Contains(rootTwelfth));
			return result;
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x00216AAC File Offset: 0x00214CAC
		public static Twelfth GetRightMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			if (twelfths.Count >= 12)
			{
				return Twelfth.Undefined;
			}
			Twelfth m;
			do
			{
				m = rootTwelfth;
				rootTwelfth = TwelfthUtility.TwelfthAfter(rootTwelfth);
			}
			while (twelfths.Contains(rootTwelfth));
			return TwelfthUtility.TwelfthAfter(m);
		}

		// Token: 0x0600604E RID: 24654 RVA: 0x00216ADF File Offset: 0x00214CDF
		public static Twelfth TwelfthBefore(Twelfth m)
		{
			if (m == Twelfth.First)
			{
				return Twelfth.Twelfth;
			}
			return (Twelfth)(m - Twelfth.Second);
		}

		// Token: 0x0600604F RID: 24655 RVA: 0x00216AEB File Offset: 0x00214CEB
		public static Twelfth TwelfthAfter(Twelfth m)
		{
			if (m == Twelfth.Twelfth)
			{
				return Twelfth.First;
			}
			return m + 1;
		}
	}
}
