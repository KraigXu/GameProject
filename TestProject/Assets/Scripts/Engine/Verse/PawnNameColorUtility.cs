using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000216 RID: 534
	public static class PawnNameColorUtility
	{
		// Token: 0x06000F10 RID: 3856 RVA: 0x0005596C File Offset: 0x00053B6C
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00055B70 File Offset: 0x00053D70
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00055BDC File Offset: 0x00053DDC
		public static Color PawnNameColorOf(Pawn pawn)
		{
			if (pawn.MentalStateDef != null)
			{
				return pawn.MentalStateDef.nameColor;
			}
			int index;
			if (pawn.Faction == null)
			{
				index = 0;
			}
			else
			{
				index = pawn.Faction.randomKey % 10;
			}
			if (pawn.IsPrisoner)
			{
				return PawnNameColorUtility.ColorsPrisoner[index];
			}
			if (pawn.IsWildMan())
			{
				return PawnNameColorUtility.ColorWildMan;
			}
			if (pawn.Faction == null)
			{
				return PawnNameColorUtility.ColorsNeutral[index];
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return PawnNameColorUtility.ColorColony;
			}
			if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return PawnNameColorUtility.ColorsHostile[index];
			}
			return PawnNameColorUtility.ColorsNeutral[index];
		}

		// Token: 0x04000B25 RID: 2853
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x04000B26 RID: 2854
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04000B27 RID: 2855
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04000B28 RID: 2856
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04000B29 RID: 2857
		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04000B2A RID: 2858
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04000B2B RID: 2859
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04000B2C RID: 2860
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04000B2D RID: 2861
		private const int ColorShiftCount = 10;

		// Token: 0x04000B2E RID: 2862
		private static readonly List<Color> ColorShifts = new List<Color>
		{
			new Color(1f, 1f, 1f),
			new Color(0.8f, 1f, 1f),
			new Color(0.8f, 0.8f, 1f),
			new Color(0.8f, 0.8f, 0.8f),
			new Color(1.2f, 1f, 1f),
			new Color(0.8f, 1.2f, 1f),
			new Color(0.8f, 1.2f, 1.2f),
			new Color(1.2f, 1.2f, 1.2f),
			new Color(1f, 1.2f, 1f),
			new Color(1.2f, 1f, 0.8f)
		};
	}
}
