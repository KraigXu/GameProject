using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000017 RID: 23
	public static class Gen
	{
		// Token: 0x06000169 RID: 361 RVA: 0x000068DC File Offset: 0x00004ADC
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000694B File Offset: 0x00004B4B
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			return (T)((object)Rand.Range(disallowFirstValue ? 1 : 0, Enum.GetValues(typeof(T)).Length));
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006977 File Offset: 0x00004B77
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006994 File Offset: 0x00004B94
		public static int GetBitCountOf(long lValue)
		{
			int num = 0;
			while (lValue != 0L)
			{
				lValue &= lValue - 1L;
				num++;
			}
			return num;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000069B5 File Offset: 0x00004BB5
		public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
		{
			CultureInfo cult = CultureInfo.InvariantCulture;
			int valueAsInt = Convert.ToInt32(value, cult);
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				int num = Convert.ToInt32(obj, cult);
				if (num == (valueAsInt & num))
				{
					yield return (T)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000069C5 File Offset: 0x00004BC5
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000069D5 File Offset: 0x00004BD5
		public static IEnumerable YieldSingleNonGeneric<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000069E8 File Offset: 0x00004BE8
		public static string ToStringSafe<T>(this T obj)
		{
			if (obj == null)
			{
				return "null";
			}
			string result;
			try
			{
				result = obj.ToString();
			}
			catch (Exception arg)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = obj.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					Log.ErrorOnce("Exception in ToString(): " + arg, num ^ 1857461521, false);
				}
				else
				{
					Log.Error("Exception in ToString(): " + arg, false);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006A84 File Offset: 0x00004C84
		public static string ToStringSafeEnumerable(this IEnumerable enumerable)
		{
			if (enumerable == null)
			{
				return "null";
			}
			string result;
			try
			{
				string text = "";
				foreach (object obj in enumerable)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += obj.ToStringSafe<object>();
				}
				result = text;
			}
			catch (Exception arg)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = enumerable.GetHashCode();
					flag = true;
				}
				catch
				{
				}
				if (flag)
				{
					Log.ErrorOnce("Exception while enumerating: " + arg, num ^ 581736153, false);
				}
				else
				{
					Log.Error("Exception while enumerating: " + arg, false);
				}
				result = "error";
			}
			return result;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006B70 File Offset: 0x00004D70
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006B97 File Offset: 0x00004D97
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006BD8 File Offset: 0x00004DD8
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006C18 File Offset: 0x00004E18
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj == null) ? 0 : obj.GetHashCode();
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006C55 File Offset: 0x00004E55
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006C7A File Offset: 0x00004E7A
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006C94 File Offset: 0x00004E94
		public static int HashCombineInt(int v1, int v2, int v3, int v4)
		{
			int num = 352654597;
			int num2 = num;
			num = ((num << 5) + num + (num >> 27) ^ v1);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v2);
			num = ((num << 5) + num + (num >> 27) ^ v3);
			num2 = ((num2 << 5) + num2 + (num2 >> 27) ^ v4);
			return num + num2 * 1566083941;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006CE6 File Offset: 0x00004EE6
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006CF3 File Offset: 0x00004EF3
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006D00 File Offset: 0x00004F00
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006D0D File Offset: 0x00004F0D
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006D1A File Offset: 0x00004F1A
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006D32 File Offset: 0x00004F32
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006D3F File Offset: 0x00004F3F
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006D57 File Offset: 0x00004F57
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006D64 File Offset: 0x00004F64
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006D7C File Offset: 0x00004F7C
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006DA8 File Offset: 0x00004FA8
		public static void ReplaceNullFields<T>(ref T replaceIn, T replaceWith)
		{
			if (replaceIn == null || replaceWith == null)
			{
				return;
			}
			foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fieldInfo.GetValue(replaceIn) == null)
				{
					object value = fieldInfo.GetValue(replaceWith);
					if (value != null)
					{
						object obj = replaceIn;
						fieldInfo.SetValue(obj, value);
						replaceIn = (T)((object)obj);
					}
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006E38 File Offset: 0x00005038
		public static void EnsureAllFieldsNullable(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsValueType && !(Nullable.GetUnderlyingType(fieldType) != null))
				{
					Log.Warning(string.Concat(new string[]
					{
						"Field ",
						type.Name,
						".",
						fieldInfo.Name,
						" is not nullable."
					}), false);
				}
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006EB8 File Offset: 0x000050B8
		public static string GetNonNullFieldsDebugInfo(object obj)
		{
			if (obj == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(fieldInfo.Name + "=" + value.ToStringSafe<object>());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400003D RID: 61
		private static MethodInfo s_memberwiseClone;
	}
}
