using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class GenDate
	{
		
		// (get) Token: 0x06005FD2 RID: 24530 RVA: 0x00214F25 File Offset: 0x00213125
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		
		// (get) Token: 0x06005FD3 RID: 24531 RVA: 0x00214F31 File Offset: 0x00213131
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		
		// (get) Token: 0x06005FD4 RID: 24532 RVA: 0x00214F3D File Offset: 0x0021313D
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		
		// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x00214F4B File Offset: 0x0021314B
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		
		// (get) Token: 0x06005FD6 RID: 24534 RVA: 0x00214F57 File Offset: 0x00213157
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		
		// (get) Token: 0x06005FD7 RID: 24535 RVA: 0x00214F65 File Offset: 0x00213165
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		
		// (get) Token: 0x06005FD8 RID: 24536 RVA: 0x00214F71 File Offset: 0x00213171
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		
		private static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		
		public static int HourOfDay(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 5);
		}

		
		public static int DayOfYear(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 60);
		}

		
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			return (Twelfth)GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 300000, 12);
		}

		
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			return SeasonUtility.GetReportedSeason(GenDate.YearPercent(absTicks, longitude), latitude);
		}

		
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			return GenDate.Twelfth(absTicks, longitude).GetQuadrum();
		}

		
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		
		public static int DayOfSeason(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		
		public static int DayTick(long absTicks, float longitude)
		{
			return (int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000L);
		}

		
		public static float DayPercent(long absTicks, float longitude)
		{
			int num = GenDate.DayTick(absTicks, longitude);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		
		public static float YearPercent(long absTicks, float longitude)
		{
			return (float)((int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 3600000L)) / 3600000f;
		}

		
		public static int HourInteger(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "FullDate".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		
		public static string DateFullStringWithHourAt(long absTicks, Vector2 location)
		{
			return GenDate.DateFullStringAt(absTicks, location) + ", " + GenDate.HourInteger(absTicks, location.x) + "LetterHour".Translate();
		}

		
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "DateReadout".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		
		public static string DateShortStringAt(long absTicks, Vector2 location)
		{
			int value = GenDate.DayOfSeason(absTicks, location.x) + 1;
			return "ShortDate".Translate(value, GenDate.Quadrum(absTicks, location.x).LabelShort(), GenDate.Year(absTicks, location.x), value);
		}

		
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Season(absTicks, longLat).Label(), num);
		}

		
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			if (text == "1")
			{
				return "Period1Day".Translate();
			}
			return text + " " + "DaysLower".Translate();
		}

		
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

		
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		
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

		
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		
		public const int TicksPerDay = 60000;

		
		public const int HoursPerDay = 24;

		
		public const int DaysPerTwelfth = 5;

		
		public const int TwelfthsPerYear = 12;

		
		public const int GameStartHourOfDay = 6;

		
		public const int TicksPerTwelfth = 300000;

		
		public const int TicksPerSeason = 900000;

		
		public const int TicksPerQuadrum = 900000;

		
		public const int TicksPerYear = 3600000;

		
		public const int DaysPerYear = 60;

		
		public const int DaysPerSeason = 15;

		
		public const int DaysPerQuadrum = 15;

		
		public const int TicksPerHour = 2500;

		
		public const float TimeZoneWidth = 15f;

		
		public const int DefaultStartingYear = 5500;
	}
}
