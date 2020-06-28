using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EA RID: 490
	public static class ZoneColorUtility
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x0004F128 File Offset: 0x0004D328
		static ZoneColorUtility()
		{
			foreach (Color color in ZoneColorUtility.GrowingZoneColors())
			{
				Color item = new Color(color.r, color.g, color.b, 0.09f);
				ZoneColorUtility.growingZoneColors.Add(item);
			}
			foreach (Color color2 in ZoneColorUtility.StorageZoneColors())
			{
				Color item2 = new Color(color2.r, color2.g, color2.b, 0.09f);
				ZoneColorUtility.storageZoneColors.Add(item2);
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0004F214 File Offset: 0x0004D414
		public static Color NextGrowingZoneColor()
		{
			Color result = ZoneColorUtility.growingZoneColors[ZoneColorUtility.nextGrowingZoneColorIndex];
			ZoneColorUtility.nextGrowingZoneColorIndex++;
			if (ZoneColorUtility.nextGrowingZoneColorIndex >= ZoneColorUtility.growingZoneColors.Count)
			{
				ZoneColorUtility.nextGrowingZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0004F248 File Offset: 0x0004D448
		public static Color NextStorageZoneColor()
		{
			Color result = ZoneColorUtility.storageZoneColors[ZoneColorUtility.nextStorageZoneColorIndex];
			ZoneColorUtility.nextStorageZoneColorIndex++;
			if (ZoneColorUtility.nextStorageZoneColorIndex >= ZoneColorUtility.storageZoneColors.Count)
			{
				ZoneColorUtility.nextStorageZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0004F27C File Offset: 0x0004D47C
		private static IEnumerable<Color> GrowingZoneColors()
		{
			yield return Color.Lerp(new Color(0f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0.5f), Color.gray, 0.5f);
			yield break;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0004F285 File Offset: 0x0004D485
		private static IEnumerable<Color> StorageZoneColors()
		{
			yield return Color.Lerp(new Color(1f, 0f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0.5f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 0f, 1f), Color.gray, 0.5f);
			yield break;
		}

		// Token: 0x04000A92 RID: 2706
		private static List<Color> growingZoneColors = new List<Color>();

		// Token: 0x04000A93 RID: 2707
		private static List<Color> storageZoneColors = new List<Color>();

		// Token: 0x04000A94 RID: 2708
		private static int nextGrowingZoneColorIndex = 0;

		// Token: 0x04000A95 RID: 2709
		private static int nextStorageZoneColorIndex = 0;

		// Token: 0x04000A96 RID: 2710
		private const float ZoneOpacity = 0.09f;
	}
}
