using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001239 RID: 4665
	public static class CaravanNightRestUtility
	{
		// Token: 0x06006CAB RID: 27819 RVA: 0x0025ED5D File Offset: 0x0025CF5D
		public static bool RestingNowAt(int tile)
		{
			return CaravanNightRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x0025ED6C File Offset: 0x0025CF6C
		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x0025EDA2 File Offset: 0x0025CFA2
		public static int LeftRestTicksAt(int tile)
		{
			return CaravanNightRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x0025EDB0 File Offset: 0x0025CFB0
		public static int LeftRestTicksAt(int tile, long ticksAbs)
		{
			if (!CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			if (num < 6f)
			{
				return Mathf.CeilToInt((6f - num) * 2500f);
			}
			return Mathf.CeilToInt((24f - num + 6f) * 2500f);
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x0025EE12 File Offset: 0x0025D012
		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanNightRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x0025EE20 File Offset: 0x0025D020
		public static int LeftNonRestTicksAt(int tile, long ticksAbs)
		{
			if (CaravanNightRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				return 0;
			}
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return Mathf.CeilToInt((22f - num) * 2500f);
		}

		// Token: 0x04004396 RID: 17302
		public const float WakeUpHour = 6f;

		// Token: 0x04004397 RID: 17303
		public const float RestStartHour = 22f;
	}
}
