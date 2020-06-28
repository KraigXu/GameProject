using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000158 RID: 344
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x00034848 File Offset: 0x00032A48
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00034948 File Offset: 0x00032B48
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x000349B2 File Offset: 0x00032BB2
		public static Material Mat(int ind, bool transparent)
		{
			if (ind >= 100)
			{
				ind = 99;
			}
			if (ind < 0)
			{
				ind = 0;
			}
			if (!transparent)
			{
				return DebugMatsSpectrum.spectrumMatsOpaque[ind];
			}
			return DebugMatsSpectrum.spectrumMatsTranparent[ind];
		}

		// Token: 0x040007EE RID: 2030
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x040007EF RID: 2031
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x040007F0 RID: 2032
		public const int MaterialCount = 100;

		// Token: 0x040007F1 RID: 2033
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};
	}
}
