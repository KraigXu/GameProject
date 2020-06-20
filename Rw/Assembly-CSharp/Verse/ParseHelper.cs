using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorld.QuestGen;
using Steamworks;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000029 RID: 41
	public static class ParseHelper
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000E357 File Offset: 0x0000C557
		public static string ParseString(string str)
		{
			return str.Replace("\\n", "\n");
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E36C File Offset: 0x0000C56C
		public static int ParseIntPermissive(string str)
		{
			int result;
			if (!int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = (int)float.Parse(str, CultureInfo.InvariantCulture);
				Log.Warning("Parsed " + str + " as int.", false);
			}
			return result;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public static Vector3 FromStringVector3(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float z = Convert.ToSingle(array[2], invariantCulture);
			return new Vector3(x, y, z);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E424 File Offset: 0x0000C624
		public static Vector2 FromStringVector2(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x;
			float y;
			if (array.Length == 1)
			{
				y = (x = Convert.ToSingle(array[0], invariantCulture));
			}
			else
			{
				if (array.Length != 2)
				{
					throw new InvalidOperationException();
				}
				x = Convert.ToSingle(array[0], invariantCulture);
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			return new Vector2(x, y);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000E4AC File Offset: 0x0000C6AC
		public static Vector4 FromStringVector4Adaptive(string Str)
		{
			Str = Str.TrimStart(new char[]
			{
				'('
			});
			Str = Str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = Str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = 0f;
			float y = 0f;
			float z = 0f;
			float w = 0f;
			if (array.Length >= 1)
			{
				x = Convert.ToSingle(array[0], invariantCulture);
			}
			if (array.Length >= 2)
			{
				y = Convert.ToSingle(array[1], invariantCulture);
			}
			if (array.Length >= 3)
			{
				z = Convert.ToSingle(array[2], invariantCulture);
			}
			if (array.Length >= 4)
			{
				w = Convert.ToSingle(array[3], invariantCulture);
			}
			if (array.Length >= 5)
			{
				Log.ErrorOnce(string.Format("Too many elements in vector {0}", Str), 16139142, false);
			}
			return new Vector4(x, y, z, w);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E57C File Offset: 0x0000C77C
		public static Rect FromStringRect(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			float x = Convert.ToSingle(array[0], invariantCulture);
			float y = Convert.ToSingle(array[1], invariantCulture);
			float width = Convert.ToSingle(array[2], invariantCulture);
			float height = Convert.ToSingle(array[3], invariantCulture);
			return new Rect(x, y, width, height);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		public static float ParseFloat(string str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E605 File Offset: 0x0000C805
		public static bool ParseBool(string str)
		{
			return bool.Parse(str);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000E60D File Offset: 0x0000C80D
		public static long ParseLong(string str)
		{
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000E61A File Offset: 0x0000C81A
		public static double ParseDouble(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E627 File Offset: 0x0000C827
		public static sbyte ParseSByte(string str)
		{
			return sbyte.Parse(str, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E634 File Offset: 0x0000C834
		public static Type ParseType(string str)
		{
			if (str == "null" || str == "Null")
			{
				return null;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(str, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Could not find a type named " + str, false);
			}
			return typeInAnyAssembly;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E674 File Offset: 0x0000C874
		public static Action ParseAction(string str)
		{
			string[] array = str.Split(new char[]
			{
				'.'
			});
			string methodName = array[array.Length - 1];
			string typeName;
			if (array.Length == 3)
			{
				typeName = array[0] + "." + array[1];
			}
			else
			{
				typeName = array[0];
			}
			MethodInfo method = GenTypes.GetTypeInAnyAssembly(typeName, null).GetMethods().First((MethodInfo m) => m.Name == methodName);
			return (Action)Delegate.CreateDelegate(typeof(Action), method);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E6F8 File Offset: 0x0000C8F8
		public static Color ParseColor(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(new char[]
			{
				','
			});
			float num = ParseHelper.ParseFloat(array[0]);
			float num2 = ParseHelper.ParseFloat(array[1]);
			float num3 = ParseHelper.ParseFloat(array[2]);
			bool flag = num > 1f || num3 > 1f || num2 > 1f;
			float num4 = (float)(flag ? 255 : 1);
			if (array.Length == 4)
			{
				num4 = ParseHelper.FromString<float>(array[3]);
			}
			Color result;
			if (!flag)
			{
				result.r = num;
				result.g = num2;
				result.b = num3;
				result.a = num4;
			}
			else
			{
				result = GenColor.FromBytes(Mathf.RoundToInt(num), Mathf.RoundToInt(num2), Mathf.RoundToInt(num3), Mathf.RoundToInt(num4));
			}
			return result;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E7CC File Offset: 0x0000C9CC
		public static PublishedFileId_t ParsePublishedFileId(string str)
		{
			return new PublishedFileId_t(ulong.Parse(str));
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E7D9 File Offset: 0x0000C9D9
		public static IntVec2 ParseIntVec2(string str)
		{
			return IntVec2.FromString(str);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E7E1 File Offset: 0x0000C9E1
		public static IntVec3 ParseIntVec3(string str)
		{
			return IntVec3.FromString(str);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E7E9 File Offset: 0x0000C9E9
		public static Rot4 ParseRot4(string str)
		{
			return Rot4.FromString(str);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E7F1 File Offset: 0x0000C9F1
		public static CellRect ParseCellRect(string str)
		{
			return CellRect.FromString(str);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000E7F9 File Offset: 0x0000C9F9
		public static CurvePoint ParseCurvePoint(string str)
		{
			return CurvePoint.FromString(str);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000E801 File Offset: 0x0000CA01
		public static NameTriple ParseNameTriple(string str)
		{
			NameTriple nameTriple = NameTriple.FromString(str);
			nameTriple.ResolveMissingPieces(null);
			return nameTriple;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000E810 File Offset: 0x0000CA10
		public static FloatRange ParseFloatRange(string str)
		{
			return FloatRange.FromString(str);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000E818 File Offset: 0x0000CA18
		public static IntRange ParseIntRange(string str)
		{
			return IntRange.FromString(str);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000E820 File Offset: 0x0000CA20
		public static QualityRange ParseQualityRange(string str)
		{
			return QualityRange.FromString(str);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000E828 File Offset: 0x0000CA28
		public static ColorInt ParseColorInt(string str)
		{
			str = str.TrimStart(ParseHelper.colorTrimStartParameters);
			str = str.TrimEnd(ParseHelper.colorTrimEndParameters);
			string[] array = str.Split(new char[]
			{
				','
			});
			ColorInt result = new ColorInt(255, 255, 255, 255);
			result.r = ParseHelper.ParseIntPermissive(array[0]);
			result.g = ParseHelper.ParseIntPermissive(array[1]);
			result.b = ParseHelper.ParseIntPermissive(array[2]);
			if (array.Length == 4)
			{
				result.a = ParseHelper.ParseIntPermissive(array[3]);
			}
			else
			{
				result.a = 255;
			}
			return result;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000E8CD File Offset: 0x0000CACD
		public static TaggedString ParseTaggedString(string str)
		{
			return str;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
		static ParseHelper()
		{
			ParseHelper.Parsers<string>.Register(new Func<string, string>(ParseHelper.ParseString));
			ParseHelper.Parsers<int>.Register(new Func<string, int>(ParseHelper.ParseIntPermissive));
			ParseHelper.Parsers<Vector3>.Register(new Func<string, Vector3>(ParseHelper.FromStringVector3));
			ParseHelper.Parsers<Vector2>.Register(new Func<string, Vector2>(ParseHelper.FromStringVector2));
			ParseHelper.Parsers<Vector4>.Register(new Func<string, Vector4>(ParseHelper.FromStringVector4Adaptive));
			ParseHelper.Parsers<Rect>.Register(new Func<string, Rect>(ParseHelper.FromStringRect));
			ParseHelper.Parsers<float>.Register(new Func<string, float>(ParseHelper.ParseFloat));
			ParseHelper.Parsers<bool>.Register(new Func<string, bool>(ParseHelper.ParseBool));
			ParseHelper.Parsers<long>.Register(new Func<string, long>(ParseHelper.ParseLong));
			ParseHelper.Parsers<double>.Register(new Func<string, double>(ParseHelper.ParseDouble));
			ParseHelper.Parsers<sbyte>.Register(new Func<string, sbyte>(ParseHelper.ParseSByte));
			ParseHelper.Parsers<Type>.Register(new Func<string, Type>(ParseHelper.ParseType));
			ParseHelper.Parsers<Action>.Register(new Func<string, Action>(ParseHelper.ParseAction));
			ParseHelper.Parsers<Color>.Register(new Func<string, Color>(ParseHelper.ParseColor));
			ParseHelper.Parsers<PublishedFileId_t>.Register(new Func<string, PublishedFileId_t>(ParseHelper.ParsePublishedFileId));
			ParseHelper.Parsers<IntVec2>.Register(new Func<string, IntVec2>(ParseHelper.ParseIntVec2));
			ParseHelper.Parsers<IntVec3>.Register(new Func<string, IntVec3>(ParseHelper.ParseIntVec3));
			ParseHelper.Parsers<Rot4>.Register(new Func<string, Rot4>(ParseHelper.ParseRot4));
			ParseHelper.Parsers<CellRect>.Register(new Func<string, CellRect>(ParseHelper.ParseCellRect));
			ParseHelper.Parsers<CurvePoint>.Register(new Func<string, CurvePoint>(ParseHelper.ParseCurvePoint));
			ParseHelper.Parsers<NameTriple>.Register(new Func<string, NameTriple>(ParseHelper.ParseNameTriple));
			ParseHelper.Parsers<FloatRange>.Register(new Func<string, FloatRange>(ParseHelper.ParseFloatRange));
			ParseHelper.Parsers<IntRange>.Register(new Func<string, IntRange>(ParseHelper.ParseIntRange));
			ParseHelper.Parsers<QualityRange>.Register(new Func<string, QualityRange>(ParseHelper.ParseQualityRange));
			ParseHelper.Parsers<ColorInt>.Register(new Func<string, ColorInt>(ParseHelper.ParseColorInt));
			ParseHelper.Parsers<TaggedString>.Register(new Func<string, TaggedString>(ParseHelper.ParseTaggedString));
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
		public static T FromString<T>(string str)
		{
			Func<string, T> parser = ParseHelper.Parsers<T>.parser;
			if (parser != null)
			{
				return parser(str);
			}
			return (T)((object)ParseHelper.FromString(str, typeof(T)));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EB04 File Offset: 0x0000CD04
		public static object FromString(string str, Type itemType)
		{
			object result;
			try
			{
				itemType = (Nullable.GetUnderlyingType(itemType) ?? itemType);
				if (itemType.IsEnum)
				{
					try
					{
						object obj = BackCompatibility.BackCompatibleEnum(itemType, str);
						if (obj != null)
						{
							return obj;
						}
						return Enum.Parse(itemType, str);
					}
					catch (ArgumentException innerException)
					{
						throw new ArgumentException(string.Concat(new object[]
						{
							"'",
							str,
							"' is not a valid value for ",
							itemType,
							". Valid values are: \n"
						}) + GenText.StringFromEnumerable(Enum.GetValues(itemType)), innerException);
					}
				}
				Func<string, object> func;
				if (ParseHelper.parsers.TryGetValue(itemType, out func))
				{
					result = func(str);
				}
				else
				{
					if (!typeof(ISlateRef).IsAssignableFrom(itemType))
					{
						throw new ArgumentException(string.Concat(new string[]
						{
							"Trying to parse to unknown data type ",
							itemType.Name,
							". Content is '",
							str,
							"'."
						}));
					}
					ISlateRef slateRef = (ISlateRef)Activator.CreateInstance(itemType);
					slateRef.SlateRef = str;
					result = slateRef;
				}
			}
			catch (Exception innerException2)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"Exception parsing ",
					itemType,
					" from \"",
					str,
					"\""
				}), innerException2);
			}
			return result;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public static bool HandlesType(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return type.IsPrimitive || type.IsEnum || ParseHelper.parsers.ContainsKey(type) || typeof(ISlateRef).IsAssignableFrom(type);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EC94 File Offset: 0x0000CE94
		public static bool CanParse(Type type, string str)
		{
			if (!ParseHelper.HandlesType(type))
			{
				return false;
			}
			try
			{
				ParseHelper.FromString(str, type);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (FormatException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x04000077 RID: 119
		private static Dictionary<Type, Func<string, object>> parsers = new Dictionary<Type, Func<string, object>>();

		// Token: 0x04000078 RID: 120
		private static readonly char[] colorTrimStartParameters = new char[]
		{
			'(',
			'R',
			'G',
			'B',
			'A'
		};

		// Token: 0x04000079 RID: 121
		private static readonly char[] colorTrimEndParameters = new char[]
		{
			')'
		};

		// Token: 0x02001305 RID: 4869
		public static class Parsers<T>
		{
			// Token: 0x06007380 RID: 29568 RVA: 0x00282394 File Offset: 0x00280594
			public static void Register(Func<string, T> method)
			{
				ParseHelper.Parsers<T>.parser = method;
				ParseHelper.parsers.Add(typeof(T), (string str) => method(str));
			}

			// Token: 0x04004807 RID: 18439
			public static Func<string, T> parser;

			// Token: 0x04004808 RID: 18440
			public static readonly string profilerLabel = "ParseHelper.FromString<" + typeof(T).FullName + ">()";
		}
	}
}
