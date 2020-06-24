using System;
using RimWorld;
using UnityEngine;

namespace Spirit
{
	// Token: 0x0200044D RID: 1101
	public static class GenTicks
	{
		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x000C9C90 File Offset: 0x000C7E90
		public static int TicksAbs
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					return GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksAbs;
				}
				return 0;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000C9CDD File Offset: 0x000C7EDD
		public static int TicksGame
		{
			get
			{
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksGame;
				}
				return 0;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000C9CFC File Offset: 0x000C7EFC
		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out result))
				{
					return result;
				}
				Vector2 vector;
				if (gameInitData.startingTile >= 0)
				{
					vector = Find.WorldGrid.LongLatOf(gameInitData.startingTile);
				}
				else
				{
					vector = Vector2.zero;
				}
				Twelfth twelfth;
				if (gameInitData.startingSeason != Season.Undefined)
				{
					twelfth = gameInitData.startingSeason.GetFirstTwelfth(vector.y);
				}
				else if (gameInitData.startingTile >= 0)
				{
					twelfth = TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile);
				}
				else
				{
					twelfth = Season.Summer.GetFirstTwelfth(0f);
				}
				int num = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
				int num2 = 300000 * (int)twelfth + 2500 * (6 + num);
				ticksAbsCache.Cache(num2, gameInitData);
				return num2;
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000C9DBE File Offset: 0x000C7FBE
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000C9DC8 File Offset: 0x000C7FC8
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000C9DD8 File Offset: 0x000C7FD8
		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000C9E18 File Offset: 0x000C8018
		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x04001417 RID: 5143
		public const int TicksPerRealSecond = 60;

		// Token: 0x04001418 RID: 5144
		public const int TickRareInterval = 250;

		// Token: 0x04001419 RID: 5145
		public const int TickLongInterval = 2000;
	}
}
