using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F40 RID: 3904
	public static class GenCelestial
	{
		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06005FC3 RID: 24515 RVA: 0x00214838 File Offset: 0x00212A38
		private static int TicksAbsForSunPosInWorldSpace
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					return GenTicks.TicksAbs;
				}
				int startingTile = Find.GameInitData.startingTile;
				float longitude = (startingTile >= 0) ? Find.WorldGrid.LongLatOf(startingTile).x : 0f;
				return Mathf.RoundToInt(2500f * (12f - GenDate.TimeZoneFloatAt(longitude)));
			}
		}

		// Token: 0x06005FC4 RID: 24516 RVA: 0x00214890 File Offset: 0x00212A90
		public static float CurCelestialSunGlow(Map map)
		{
			return GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs);
		}

		// Token: 0x06005FC5 RID: 24517 RVA: 0x002148A4 File Offset: 0x00212AA4
		public static float CelestialSunGlow(Map map, int ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenDate.DayOfYear((long)ticksAbs, vector.x), GenDate.DayPercent((long)ticksAbs, vector.x));
		}

		// Token: 0x06005FC6 RID: 24518 RVA: 0x002148E7 File Offset: 0x00212AE7
		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01(Mathf.Abs(GenCelestial.CurCelestialSunGlow(map) - 0.6f) / 0.15f);
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x00214908 File Offset: 0x00212B08
		public static GenCelestial.LightInfo GetLightSourceInfo(Map map, GenCelestial.LightType type)
		{
			float num = GenLocalDate.DayPercent(map);
			bool flag;
			float intensity;
			if (type == GenCelestial.LightType.Shadow)
			{
				flag = GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map));
				intensity = GenCelestial.CurShadowStrength(map);
			}
			else if (type == GenCelestial.LightType.LightingSun)
			{
				flag = true;
				intensity = Mathf.Clamp01((GenCelestial.CurCelestialSunGlow(map) - 0.6f + 0.2f) / 0.15f);
			}
			else if (type == GenCelestial.LightType.LightingMoon)
			{
				flag = false;
				intensity = Mathf.Clamp01(-(GenCelestial.CurCelestialSunGlow(map) - 0.6f - 0.2f) / 0.15f);
			}
			else
			{
				Log.ErrorOnce("Invalid light type requested", 64275614, false);
				flag = true;
				intensity = 0f;
			}
			float t;
			float num2;
			float num3;
			if (flag)
			{
				t = num;
				num2 = -1.5f;
				num3 = 15f;
			}
			else
			{
				if (num > 0.5f)
				{
					t = Mathf.InverseLerp(0.5f, 1f, num) * 0.5f;
				}
				else
				{
					t = 0.5f + Mathf.InverseLerp(0f, 0.5f, num) * 0.5f;
				}
				num2 = -0.9f;
				num3 = 15f;
			}
			float num4 = Mathf.LerpUnclamped(-num3, num3, t);
			float y = num2 - 2.5f * (num4 * num4 / 100f);
			return new GenCelestial.LightInfo
			{
				vector = new Vector2(num4, y),
				intensity = intensity
			};
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x00214A40 File Offset: 0x00212C40
		public static Vector3 CurSunPositionInWorldSpace()
		{
			int ticksAbsForSunPosInWorldSpace = GenCelestial.TicksAbsForSunPosInWorldSpace;
			return GenCelestial.SunPositionUnmodified((float)GenDate.DayOfYear((long)ticksAbsForSunPosInWorldSpace, 0f), GenDate.DayPercent((long)ticksAbsForSunPosInWorldSpace, 0f), new Vector3(0f, 0f, -1f), 0f);
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x00214A8A File Offset: 0x00212C8A
		public static bool IsDaytime(float glow)
		{
			return glow > 0.6f;
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x00214A94 File Offset: 0x00212C94
		private static Vector3 SunPosition(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 target = GenCelestial.SurfaceNormal(latitude);
			Vector3 current = GenCelestial.SunPositionUnmodified((float)dayOfYear, dayPercent, new Vector3(1f, 0f, 0f), latitude);
			float num = GenCelestial.SunPeekAroundDegreesFactorCurve.Evaluate(latitude);
			current = Vector3.RotateTowards(current, target, 0.331612557f * num, 9999999f);
			float num2 = Mathf.InverseLerp(60f, 0f, Mathf.Abs(latitude));
			if (num2 > 0f)
			{
				current = Vector3.RotateTowards(current, target, 6.28318548f * (17f * num2 / 360f), 9999999f);
			}
			return current.normalized;
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x00214B2C File Offset: 0x00212D2C
		private static Vector3 SunPositionUnmodified(float dayOfYear, float dayPercent, Vector3 initialSunPos, float latitude = 0f)
		{
			Vector3 point = initialSunPos * 100f;
			float num = -Mathf.Cos(dayOfYear / 60f * 3.14159274f * 2f);
			point.y += num * 100f * GenCelestial.SunOffsetFractionFromLatitudeCurve.Evaluate(latitude);
			point = Quaternion.AngleAxis((dayPercent - 0.5f) * 360f, Vector3.up) * point;
			return point.normalized;
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x00214BA4 File Offset: 0x00212DA4
		private static float CelestialSunGlowPercent(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 vector = GenCelestial.SurfaceNormal(latitude);
			Vector3 rhs = GenCelestial.SunPosition(latitude, dayOfYear, dayPercent);
			float value = Vector3.Dot(vector.normalized, rhs);
			return Mathf.Clamp01(Mathf.InverseLerp(0f, 0.7f, value));
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x00214BE4 File Offset: 0x00212DE4
		public static float AverageGlow(float latitude, int dayOfYear)
		{
			float num = 0f;
			for (int i = 0; i < 24; i++)
			{
				num += GenCelestial.CelestialSunGlowPercent(latitude, dayOfYear, (float)i / 24f);
			}
			return num / 24f;
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x00214C20 File Offset: 0x00212E20
		private static Vector3 SurfaceNormal(float latitude)
		{
			Vector3 vector = new Vector3(1f, 0f, 0f);
			vector = Quaternion.AngleAxis(latitude, new Vector3(0f, 0f, 1f)) * vector;
			return vector;
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x00214C68 File Offset: 0x00212E68
		public static void LogSunGlowForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun visibility percents for latitude " + i + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)i, k, (float)l / 24f).ToString("F2").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x00214D70 File Offset: 0x00212F70
		public static void LogSunAngleForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun angles for latitude " + i + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						float num = Vector3.Angle(GenCelestial.SurfaceNormal((float)i), GenCelestial.SunPositionUnmodified((float)k, (float)l / 24f, new Vector3(1f, 0f, 0f), 0f));
						stringBuilder.Append((90f - num).ToString("F0").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		// Token: 0x040033F7 RID: 13303
		public const float ShadowMaxLengthDay = 15f;

		// Token: 0x040033F8 RID: 13304
		public const float ShadowMaxLengthNight = 15f;

		// Token: 0x040033F9 RID: 13305
		private const float ShadowGlowLerpSpan = 0.15f;

		// Token: 0x040033FA RID: 13306
		private const float ShadowDayNightThreshold = 0.6f;

		// Token: 0x040033FB RID: 13307
		private static SimpleCurve SunPeekAroundDegreesFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.05f),
				true
			}
		};

		// Token: 0x040033FC RID: 13308
		private static SimpleCurve SunOffsetFractionFromLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 0.2f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x02001E57 RID: 7767
		public struct LightInfo
		{
			// Token: 0x04007224 RID: 29220
			public Vector2 vector;

			// Token: 0x04007225 RID: 29221
			public float intensity;
		}

		// Token: 0x02001E58 RID: 7768
		public enum LightType
		{
			// Token: 0x04007227 RID: 29223
			Shadow,
			// Token: 0x04007228 RID: 29224
			LightingSun,
			// Token: 0x04007229 RID: 29225
			LightingMoon
		}
	}
}
