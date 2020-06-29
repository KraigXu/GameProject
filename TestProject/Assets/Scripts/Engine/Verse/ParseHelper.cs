using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using RimWorld;
using RimWorld.QuestGenNew;
using Steamworks;
using UnityEngine;

namespace Verse
{
	
	public static class ParseHelper
	{
		
		public static string ParseString(string str)
		{
			return str.Replace("\\n", "\n");
		}

		
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

		
		public static float ParseFloat(string str)
		{
			return float.Parse(str, CultureInfo.InvariantCulture);
		}

		
		public static bool ParseBool(string str)
		{
			return bool.Parse(str);
		}

		
		public static long ParseLong(string str)
		{
			return long.Parse(str, CultureInfo.InvariantCulture);
		}

		
		public static double ParseDouble(string str)
		{
			return double.Parse(str, CultureInfo.InvariantCulture);
		}

		
		public static sbyte ParseSByte(string str)
		{
			return sbyte.Parse(str, CultureInfo.InvariantCulture);
		}

		
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

		
		public static PublishedFileId_t ParsePublishedFileId(string str)
		{
			return new PublishedFileId_t(ulong.Parse(str));
		}

		
		public static IntVec2 ParseIntVec2(string str)
		{
			return IntVec2.FromString(str);
		}

		
		public static IntVec3 ParseIntVec3(string str)
		{
			return IntVec3.FromString(str);
		}

		
		public static Rot4 ParseRot4(string str)
		{
			return Rot4.FromString(str);
		}

		
		public static CellRect ParseCellRect(string str)
		{
			return CellRect.FromString(str);
		}

		
		public static CurvePoint ParseCurvePoint(string str)
		{
			return CurvePoint.FromString(str);
		}

		
		public static NameTriple ParseNameTriple(string str)
		{
			NameTriple nameTriple = NameTriple.FromString(str);
			nameTriple.ResolveMissingPieces(null);
			return nameTriple;
		}

		
		public static FloatRange ParseFloatRange(string str)
		{
			return FloatRange.FromString(str);
		}

		
		public static IntRange ParseIntRange(string str)
		{
			return IntRange.FromString(str);
		}

		
		public static QualityRange ParseQualityRange(string str)
		{
			return QualityRange.FromString(str);
		}

		
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

		
		public static TaggedString ParseTaggedString(string str)
		{
			return str;
		}

		
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

		
		public static T FromString<T>(string str)
		{
			Func<string, T> parser = ParseHelper.Parsers<T>.parser;
			if (parser != null)
			{
				return parser(str);
			}
			return (T)((object)ParseHelper.FromString(str, typeof(T)));
		}

		
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

		
		public static bool HandlesType(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			return type.IsPrimitive || type.IsEnum || ParseHelper.parsers.ContainsKey(type) || typeof(ISlateRef).IsAssignableFrom(type);
		}

		
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

		
		private static Dictionary<Type, Func<string, object>> parsers = new Dictionary<Type, Func<string, object>>();

		
		private static readonly char[] colorTrimStartParameters = new char[]
		{
			'(',
			'R',
			'G',
			'B',
			'A'
		};

		
		private static readonly char[] colorTrimEndParameters = new char[]
		{
			')'
		};

		
		public static class Parsers<T>
		{
			
			public static void Register(Func<string, T> method)
			{
				ParseHelper.Parsers<T>.parser = method;
				ParseHelper.parsers.Add(typeof(T), (string str) => method(str));
			}

			
			public static Func<string, T> parser;

			
			public static readonly string profilerLabel = "ParseHelper.FromString<" + typeof(T).FullName + ">()";
		}
	}
}
