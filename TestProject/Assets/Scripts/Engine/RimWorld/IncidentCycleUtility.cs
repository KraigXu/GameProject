﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C6 RID: 2502
	public class IncidentCycleUtility
	{
		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003BBA RID: 15290 RVA: 0x0013B2B7 File Offset: 0x001394B7
		private static int QueueIntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0013B2CC File Offset: 0x001394CC
		public static int IncidentCountThisInterval(IIncidentTarget target, int randSeedSalt, float minDaysPassed, float onDays, float offDays, float minSpacingDays, float minIncidents, float maxIncidents, float acceptFraction = 1f)
		{
			int num = IncidentCycleUtility.DaysToIntervals(minDaysPassed);
			int num2 = IncidentCycleUtility.QueueIntervalsPassed - num;
			if (num2 < 0)
			{
				return 0;
			}
			int num3 = IncidentCycleUtility.DaysToIntervals(onDays);
			int num4 = IncidentCycleUtility.DaysToIntervals(offDays);
			int minSpacingIntervals = IncidentCycleUtility.DaysToIntervals(minSpacingDays);
			int num5 = num3 + num4;
			int num6 = num2 / num5;
			int fixedHit = -9999999;
			for (int i = 0; i <= num6; i++)
			{
				int seed = Gen.HashCombineInt(Find.World.info.persistentRandomValue, target.ConstantRandSeed, randSeedSalt, i);
				int start = i * num5;
				if (IncidentCycleUtility.hits.Count > 0)
				{
					fixedHit = IncidentCycleUtility.hits[IncidentCycleUtility.hits.Count - 1];
				}
				IncidentCycleUtility.hits.Clear();
				IncidentCycleUtility.GenerateHitList(seed, start, num3, minIncidents, maxIncidents, minSpacingIntervals, acceptFraction, fixedHit);
			}
			int num7 = 0;
			for (int j = 0; j < IncidentCycleUtility.hits.Count; j++)
			{
				if (IncidentCycleUtility.hits[j] == num2)
				{
					num7++;
				}
			}
			IncidentCycleUtility.hits.Clear();
			return num7;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0013B3CC File Offset: 0x001395CC
		private static void GenerateHitList(int seed, int start, int length, float minIncidents, float maxIncidents, int minSpacingIntervals, float acceptFraction, int fixedHit)
		{
			if (IncidentCycleUtility.hits.Count > 0)
			{
				throw new Exception();
			}
			Rand.PushState();
			Rand.Seed = seed;
			int num = GenMath.RoundRandom(Rand.Range(minIncidents, maxIncidents));
			int num2 = 0;
			for (;;)
			{
				IncidentCycleUtility.hits.Clear();
				if (num2++ > 100)
				{
					break;
				}
				for (int i = 0; i < num; i++)
				{
					int item = Rand.Range(0, length) + start;
					IncidentCycleUtility.hits.Add(item);
				}
				IncidentCycleUtility.hits.Sort();
				if (IncidentCycleUtility.RelaxToSatisfyMinDiff(IncidentCycleUtility.hits, minSpacingIntervals, fixedHit, start + length))
				{
					goto IL_91;
				}
			}
			Log.ErrorOnce("Too many tries finding incident time. minSpacingDays is too high.", 12612131, false);
			IL_91:
			if (acceptFraction < 1f)
			{
				int num3 = GenMath.RoundRandom((float)IncidentCycleUtility.hits.Count * acceptFraction);
				IncidentCycleUtility.hits.Shuffle<int>();
				IncidentCycleUtility.hits.RemoveRange(num3, IncidentCycleUtility.hits.Count - num3);
			}
			Rand.PopState();
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0013B4B0 File Offset: 0x001396B0
		private static bool RelaxToSatisfyMinDiff(List<int> values, int minDiff, int fixedValue, int max)
		{
			if (values.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < values.Count; i++)
			{
				int num = (i == 0) ? Mathf.Abs(values[i] - fixedValue) : Mathf.Abs(values[i] - values[i - 1]);
				if (num < minDiff)
				{
					int index = i;
					values[index] += minDiff - num;
					for (int j = i + 1; j < values.Count; j++)
					{
						if (values[j] < values[i] + j)
						{
							values[j] = values[i] + j;
						}
					}
				}
			}
			return values[values.Count - 1] <= max;
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0013B573 File Offset: 0x00139773
		private static int DaysToIntervals(float days)
		{
			return Mathf.RoundToInt(days * 60f);
		}

		// Token: 0x04002339 RID: 9017
		private static List<int> hits = new List<int>();
	}
}
