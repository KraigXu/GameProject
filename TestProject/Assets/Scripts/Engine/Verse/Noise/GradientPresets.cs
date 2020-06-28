using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020004A2 RID: 1186
	public static class GradientPresets
	{
		// Token: 0x0600230E RID: 8974 RVA: 0x000D407C File Offset: 0x000D227C
		static GradientPresets()
		{
			List<GradientColorKey> list = new List<GradientColorKey>();
			list.Add(new GradientColorKey(Color.black, 0f));
			list.Add(new GradientColorKey(Color.white, 1f));
			List<GradientColorKey> list2 = new List<GradientColorKey>();
			list2.Add(new GradientColorKey(Color.red, 0f));
			list2.Add(new GradientColorKey(Color.green, 0.5f));
			list2.Add(new GradientColorKey(Color.blue, 1f));
			List<GradientColorKey> list3 = new List<GradientColorKey>();
			list3.Add(new GradientColorKey(Color.red, 0f));
			list3.Add(new GradientColorKey(Color.green, 0.333333343f));
			list3.Add(new GradientColorKey(Color.blue, 0.6666667f));
			list3.Add(new GradientColorKey(Color.black, 1f));
			List<GradientAlphaKey> list4 = new List<GradientAlphaKey>();
			list4.Add(new GradientAlphaKey(0f, 0.6666667f));
			list4.Add(new GradientAlphaKey(1f, 1f));
			List<GradientColorKey> list5 = new List<GradientColorKey>();
			list5.Add(new GradientColorKey(new Color(0f, 0f, 0.5f), 0f));
			list5.Add(new GradientColorKey(new Color(0.125f, 0.25f, 0.5f), 0.4f));
			list5.Add(new GradientColorKey(new Color(0.25f, 0.375f, 0.75f), 0.48f));
			list5.Add(new GradientColorKey(new Color(0f, 0.75f, 0f), 0.5f));
			list5.Add(new GradientColorKey(new Color(0.75f, 0.75f, 0f), 0.625f));
			list5.Add(new GradientColorKey(new Color(0.625f, 0.375f, 0.25f), 0.75f));
			list5.Add(new GradientColorKey(new Color(0.5f, 1f, 1f), 0.875f));
			list5.Add(new GradientColorKey(Color.white, 1f));
			List<GradientAlphaKey> list6 = new List<GradientAlphaKey>();
			list6.Add(new GradientAlphaKey(1f, 0f));
			list6.Add(new GradientAlphaKey(1f, 1f));
			GradientPresets._empty = new Gradient();
			GradientPresets._rgb = new Gradient();
			GradientPresets._rgb.SetKeys(list2.ToArray(), list6.ToArray());
			GradientPresets._rgba = new Gradient();
			GradientPresets._rgba.SetKeys(list3.ToArray(), list4.ToArray());
			GradientPresets._grayscale = new Gradient();
			GradientPresets._grayscale.SetKeys(list.ToArray(), list6.ToArray());
			GradientPresets._terrain = new Gradient();
			GradientPresets._terrain.SetKeys(list5.ToArray(), list6.ToArray());
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x000D4369 File Offset: 0x000D2569
		public static Gradient Empty
		{
			get
			{
				return GradientPresets._empty;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x000D4370 File Offset: 0x000D2570
		public static Gradient Grayscale
		{
			get
			{
				return GradientPresets._grayscale;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x000D4377 File Offset: 0x000D2577
		public static Gradient RGB
		{
			get
			{
				return GradientPresets._rgb;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000D437E File Offset: 0x000D257E
		public static Gradient RGBA
		{
			get
			{
				return GradientPresets._rgba;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x000D4385 File Offset: 0x000D2585
		public static Gradient Terrain
		{
			get
			{
				return GradientPresets._terrain;
			}
		}

		// Token: 0x0400154F RID: 5455
		private static Gradient _empty;

		// Token: 0x04001550 RID: 5456
		private static Gradient _grayscale;

		// Token: 0x04001551 RID: 5457
		private static Gradient _rgb;

		// Token: 0x04001552 RID: 5458
		private static Gradient _rgba;

		// Token: 0x04001553 RID: 5459
		private static Gradient _terrain;
	}
}
