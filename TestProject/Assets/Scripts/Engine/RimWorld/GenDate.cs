using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F41 RID: 3905
	public static class GenDate
	{
		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06005FD2 RID: 24530 RVA: 0x00214F25 File Offset: 0x00213125
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06005FD3 RID: 24531 RVA: 0x00214F31 File Offset: 0x00213131
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06005FD4 RID: 24532 RVA: 0x00214F3D File Offset: 0x0021313D
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x00214F4B File Offset: 0x0021314B
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06005FD6 RID: 24534 RVA: 0x00214F57 File Offset: 0x00213157
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06005FD7 RID: 24535 RVA: 0x00214F65 File Offset: 0x00213165
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06005FD8 RID: 24536 RVA: 0x00214F71 File Offset: 0x00213171
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x00214F7F File Offset: 0x0021317F
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x00214F8D File Offset: 0x0021318D
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x06005FDB RID: 24539 RVA: 0x00214F9B File Offset: 0x0021319B
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x00214FAA File Offset: 0x002131AA
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x00214FB9 File Offset: 0x002131B9
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		// Token: 0x06005FDE RID: 24542 RVA: 0x00214FC8 File Offset: 0x002131C8
		private static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		// Token: 0x06005FDF RID: 24543 RVA: 0x00214FD8 File Offset: 0x002131D8
		public static int HourOfDay(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x00214FEE File Offset: 0x002131EE
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 5);
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x00215003 File Offset: 0x00213203
		public static int DayOfYear(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 60);
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x00215019 File Offset: 0x00213219
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			return (Twelfth)GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 300000, 12);
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x00215030 File Offset: 0x00213230
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x00215044 File Offset: 0x00213244
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			return SeasonUtility.GetReportedSeason(GenDate.YearPercent(absTicks, longitude), latitude);
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x00215053 File Offset: 0x00213253
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			return GenDate.Twelfth(absTicks, longitude).GetQuadrum();
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x00215064 File Offset: 0x00213264
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x0021508D File Offset: 0x0021328D
		public static int DayOfSeason(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x002150AB File Offset: 0x002132AB
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x002150C4 File Offset: 0x002132C4
		public static int DayTick(long absTicks, float longitude)
		{
			return (int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000L);
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x002150DC File Offset: 0x002132DC
		public static float DayPercent(long absTicks, float longitude)
		{
			int num = GenDate.DayTick(absTicks, longitude);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x002150FE File Offset: 0x002132FE
		public static float YearPercent(long absTicks, float longitude)
		{
			return (float)((int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 3600000L)) / 3600000f;
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x00214FD8 File Offset: 0x002131D8
		public static int HourInteger(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x0021511B File Offset: 0x0021331B
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x0021512C File Offset: 0x0021332C
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "FullDate".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x00215197 File Offset: 0x00213397
		public static string DateFullStringWithHourAt(long absTicks, Vector2 location)
		{
			return GenDate.DateFullStringAt(absTicks, location) + ", " + GenDate.HourInteger(absTicks, location.x) + "LetterHour".Translate();
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x002151D0 File Offset: 0x002133D0
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "DateReadout".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x0021523C File Offset: 0x0021343C
		public static string DateShortStringAt(long absTicks, Vector2 location)
		{
			int value = GenDate.DayOfSeason(absTicks, location.x) + 1;
			return "ShortDate".Translate(value, GenDate.Quadrum(absTicks, location.x).LabelShort(), GenDate.Year(absTicks, location.x), value);
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x0021529C File Offset: 0x0021349C
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Season(absTicks, longLat).Label(), num);
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x002152F1 File Offset: 0x002134F1
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		// Token: 0x06005FF4 RID: 24564 RVA: 0x00215304 File Offset: 0x00213504
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		// Token: 0x06005FF5 RID: 24565 RVA: 0x00215354 File Offset: 0x00213554
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x0021536A File Offset: 0x0021356A
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x00215380 File Offset: 0x00213580
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x0021538C File Offset: 0x0021358C
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			if (text == "1")
			{
				return "Period1Day".Translate();
			}
			return text + " " + "DaysLower".Translate();
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x002153E8 File Offset: 0x002135E8
		public static string ToStringTicksToPeriod(this int numTicks, bool allowSeconds = true, bool shortForm = false, bool canUseDecimals = true, bool allowYears = true)
		{
			if (allowSeconds && numTicks < 2500 && (numTicks < 600 || Math.Round((double)((float)numTicks / 2500f), 1) == 0.0))
			{
				int num = Mathf.RoundToInt((float)numTicks / 60f);
				if (shortForm)
				{
					return num + "LetterSecond".Translate();
				}
				if (num == 1)
				{
					return "Period1Second".Translate();
				}
				return "PeriodSeconds".Translate(num);
			}
			else if (numTicks < 60000)
			{
				if (shortForm)
				{
					return Mathf.RoundToInt((float)numTicks / 2500f) + "LetterHour".Translate();
				}
				if (numTicks < 2500)
				{
					string text = ((float)numTicks / 2500f).ToString("0.#");
					if (text == "1")
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(text);
				}
				else
				{
					int num2 = Mathf.RoundToInt((float)numTicks / 2500f);
					if (num2 == 1)
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(num2);
				}
			}
			else if (numTicks < 3600000 || !allowYears)
			{
				if (shortForm)
				{
					return Mathf.RoundToInt((float)numTicks / 60000f) + "LetterDay".Translate();
				}
				string text2;
				if (canUseDecimals)
				{
					text2 = ((float)numTicks / 60000f).ToStringDecimalIfSmall();
				}
				else
				{
					text2 = Mathf.RoundToInt((float)numTicks / 60000f).ToString();
				}
				if (text2 == "1")
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(text2);
			}
			else
			{
				if (shortForm)
				{
					return Mathf.RoundToInt((float)numTicks / 3600000f) + "LetterYear".Translate();
				}
				string text3;
				if (canUseDecimals)
				{
					text3 = ((float)numTicks / 3600000f).ToStringDecimalIfSmall();
				}
				else
				{
					text3 = Mathf.RoundToInt((float)numTicks / 3600000f).ToString();
				}
				if (text3 == "1")
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(text3);
			}
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x00215660 File Offset: 0x00213860
		public static string ToStringTicksToPeriodVerbose(this int numTicks, bool allowHours = true, bool allowQuadrums = true)
		{
			if (numTicks < 0)
			{
				return "0";
			}
			int num;
			int num2;
			int num3;
			float num4;
			numTicks.TicksToPeriod(out num, out num2, out num3, out num4);
			if (!allowQuadrums)
			{
				num3 += 15 * num2;
				num2 = 0;
			}
			if (num > 0)
			{
				string text;
				if (num == 1)
				{
					text = "Period1Year".Translate();
				}
				else
				{
					text = "PeriodYears".Translate(num);
				}
				if (num2 > 0)
				{
					text += ", ";
					if (num2 == 1)
					{
						text += "Period1Quadrum".Translate();
					}
					else
					{
						text += "PeriodQuadrums".Translate(num2);
					}
				}
				return text;
			}
			if (num2 > 0)
			{
				string text2;
				if (num2 == 1)
				{
					text2 = "Period1Quadrum".Translate();
				}
				else
				{
					text2 = "PeriodQuadrums".Translate(num2);
				}
				if (num3 > 0)
				{
					text2 += ", ";
					if (num3 == 1)
					{
						text2 += "Period1Day".Translate();
					}
					else
					{
						text2 += "PeriodDays".Translate(num3);
					}
				}
				return text2;
			}
			if (num3 > 0)
			{
				string text3;
				if (num3 == 1)
				{
					text3 = "Period1Day".Translate();
				}
				else
				{
					text3 = "PeriodDays".Translate(num3);
				}
				int num5 = (int)num4;
				if (allowHours && num5 > 0)
				{
					text3 += ", ";
					if (num5 == 1)
					{
						text3 += "Period1Hour".Translate();
					}
					else
					{
						text3 += "PeriodHours".Translate(num5);
					}
				}
				return text3;
			}
			if (!allowHours)
			{
				return "PeriodDays".Translate(0);
			}
			if (num4 > 1f)
			{
				int num6 = Mathf.RoundToInt(num4);
				if (num6 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num6);
			}
			else
			{
				if (Math.Round((double)num4, 1) == 1.0)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num4.ToString("0.#"));
			}
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x002158C4 File Offset: 0x00213AC4
		public static string ToStringTicksToPeriodVague(this int numTicks, bool vagueMin = true, bool vagueMax = true)
		{
			if (vagueMax && numTicks > 36000000)
			{
				return "OverADecade".Translate();
			}
			if (vagueMin && numTicks < 60000)
			{
				return "LessThanADay".Translate();
			}
			return numTicks.ToStringTicksToPeriod(true, false, true, true);
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x00215911 File Offset: 0x00213B11
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x00215920 File Offset: 0x00213B20
		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			if (numTicks < 0L)
			{
				Log.ErrorOnce("Tried to calculate period for negative ticks", 12841103, false);
			}
			years = (int)(numTicks / 3600000L);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000L);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000L);
			num -= (long)days * 60000L;
			hoursFloat = (float)num / 2500f;
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x00215998 File Offset: 0x00213B98
		public static string ToStringApproxAge(this float yearsFloat)
		{
			if (yearsFloat >= 1f)
			{
				return ((int)yearsFloat).ToStringCached();
			}
			int num;
			int num2;
			int num3;
			float num4;
			Mathf.Min((int)(yearsFloat * 3600000f), 3599999).TicksToPeriod(out num, out num2, out num3, out num4);
			if (num > 0)
			{
				if (num == 1)
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(num);
			}
			else if (num2 > 0)
			{
				if (num2 == 1)
				{
					return "Period1Quadrum".Translate();
				}
				return "PeriodQuadrums".Translate(num2);
			}
			else if (num3 > 0)
			{
				if (num3 == 1)
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(num3);
			}
			else
			{
				int num5 = (int)num4;
				if (num5 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num5);
			}
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x00215A8D File Offset: 0x00213C8D
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x00215A9A File Offset: 0x00213C9A
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		// Token: 0x040033FD RID: 13309
		public const int TicksPerDay = 60000;

		// Token: 0x040033FE RID: 13310
		public const int HoursPerDay = 24;

		// Token: 0x040033FF RID: 13311
		public const int DaysPerTwelfth = 5;

		// Token: 0x04003400 RID: 13312
		public const int TwelfthsPerYear = 12;

		// Token: 0x04003401 RID: 13313
		public const int GameStartHourOfDay = 6;

		// Token: 0x04003402 RID: 13314
		public const int TicksPerTwelfth = 300000;

		// Token: 0x04003403 RID: 13315
		public const int TicksPerSeason = 900000;

		// Token: 0x04003404 RID: 13316
		public const int TicksPerQuadrum = 900000;

		// Token: 0x04003405 RID: 13317
		public const int TicksPerYear = 3600000;

		// Token: 0x04003406 RID: 13318
		public const int DaysPerYear = 60;

		// Token: 0x04003407 RID: 13319
		public const int DaysPerSeason = 15;

		// Token: 0x04003408 RID: 13320
		public const int DaysPerQuadrum = 15;

		// Token: 0x04003409 RID: 13321
		public const int TicksPerHour = 2500;

		// Token: 0x0400340A RID: 13322
		public const float TimeZoneWidth = 15f;

		// Token: 0x0400340B RID: 13323
		public const int DefaultStartingYear = 5500;
	}
}
