using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000024 RID: 36
	public static class GenTypes
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000B3C4 File Offset: 0x000095C4
		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					int num;
					for (int i = 0; i < mod.assemblies.loadedAssemblies.Count; i = num + 1)
					{
						yield return mod.assemblies.loadedAssemblies[i];
						num = i;
					}
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000B3CD File Offset: 0x000095CD
		public static IEnumerable<Type> AllTypes
		{
			get
			{
				foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
				{
					Type[] array = null;
					try
					{
						array = assembly.GetTypes();
					}
					catch (ReflectionTypeLoadException)
					{
						Log.Error("Exception getting types in assembly " + assembly.ToString(), false);
					}
					if (array != null)
					{
						foreach (Type type in array)
						{
							yield return type;
						}
						Type[] array2 = null;
					}
				}
				IEnumerator<Assembly> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000B3D6 File Offset: 0x000095D6
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000B404 File Offset: 0x00009604
		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000B434 File Offset: 0x00009634
		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000B464 File Offset: 0x00009664
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000B490 File Offset: 0x00009690
		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
			}
			foreach (Type type in baseType.AllSubclasses())
			{
				if (!type.IsAbstract)
				{
					yield return type;
				}
			}
			IEnumerator<Type> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000B4A0 File Offset: 0x000096A0
		public static Type GetTypeInAnyAssembly(string typeName, string namespaceIfAmbiguous = null)
		{
			GenTypes.TypeCacheKey key = new GenTypes.TypeCacheKey(typeName, namespaceIfAmbiguous);
			Type type = null;
			if (!GenTypes.typeCache.TryGetValue(key, out type))
			{
				type = GenTypes.GetTypeInAnyAssemblyInt(typeName, namespaceIfAmbiguous);
				GenTypes.typeCache.Add(key, type);
			}
			return type;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000B4DC File Offset: 0x000096DC
		private static Type GetTypeInAnyAssemblyInt(string typeName, string namespaceIfAmbiguous = null)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (typeInAnyAssemblyRaw != null)
			{
				return typeInAnyAssemblyRaw;
			}
			if (!namespaceIfAmbiguous.NullOrEmpty() && GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(namespaceIfAmbiguous + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(GenTypes.IgnoredNamespaceNames[i] + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			return null;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000B56C File Offset: 0x0000976C
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeName);
			if (num <= 2299065237u)
			{
				if (num <= 1092586446u)
				{
					if (num <= 431052896u)
					{
						if (num != 296283782u)
						{
							if (num != 398550328u)
							{
								if (num == 431052896u)
								{
									if (typeName == "byte?")
									{
										return typeof(byte?);
									}
								}
							}
							else if (typeName == "string")
							{
								return typeof(string);
							}
						}
						else if (typeName == "char?")
						{
							return typeof(char?);
						}
					}
					else if (num != 513669818u)
					{
						if (num != 520654156u)
						{
							if (num == 1092586446u)
							{
								if (typeName == "float?")
								{
									return typeof(float?);
								}
							}
						}
						else if (typeName == "decimal")
						{
							return typeof(decimal);
						}
					}
					else if (typeName == "uint?")
					{
						return typeof(uint?);
					}
				}
				else if (num <= 1454009365u)
				{
					if (num != 1189328644u)
					{
						if (num != 1299622921u)
						{
							if (num == 1454009365u)
							{
								if (typeName == "sbyte?")
								{
									return typeof(sbyte?);
								}
							}
						}
						else if (typeName == "decimal?")
						{
							return typeof(decimal?);
						}
					}
					else if (typeName == "long?")
					{
						return typeof(long?);
					}
				}
				else if (num <= 1630192034u)
				{
					if (num != 1603400371u)
					{
						if (num == 1630192034u)
						{
							if (typeName == "ushort")
							{
								return typeof(ushort);
							}
						}
					}
					else if (typeName == "int?")
					{
						return typeof(int?);
					}
				}
				else if (num != 1683620383u)
				{
					if (num == 2299065237u)
					{
						if (typeName == "double?")
						{
							return typeof(double?);
						}
					}
				}
				else if (typeName == "byte")
				{
					return typeof(byte);
				}
			}
			else if (num <= 2823553821u)
			{
				if (num <= 2515107422u)
				{
					if (num != 2471414311u)
					{
						if (num != 2508976771u)
						{
							if (num == 2515107422u)
							{
								if (typeName == "int")
								{
									return typeof(int);
								}
							}
						}
						else if (typeName == "ulong?")
						{
							return typeof(ulong?);
						}
					}
					else if (typeName == "ushort?")
					{
						return typeof(ushort?);
					}
				}
				else if (num <= 2699759368u)
				{
					if (num != 2667225454u)
					{
						if (num == 2699759368u)
						{
							if (typeName == "double")
							{
								return typeof(double);
							}
						}
					}
					else if (typeName == "ulong")
					{
						return typeof(ulong);
					}
				}
				else if (num != 2797886853u)
				{
					if (num == 2823553821u)
					{
						if (typeName == "char")
						{
							return typeof(char);
						}
					}
				}
				else if (typeName == "float")
				{
					return typeof(float);
				}
			}
			else if (num <= 3286667814u)
			{
				if (num != 3122818005u)
				{
					if (num != 3270303571u)
					{
						if (num == 3286667814u)
						{
							if (typeName == "bool?")
							{
								return typeof(bool?);
							}
						}
					}
					else if (typeName == "long")
					{
						return typeof(long);
					}
				}
				else if (typeName == "short")
				{
					return typeof(short);
				}
			}
			else if (num <= 3415750305u)
			{
				if (num != 3365180733u)
				{
					if (num == 3415750305u)
					{
						if (typeName == "uint")
						{
							return typeof(uint);
						}
					}
				}
				else if (typeName == "bool")
				{
					return typeof(bool);
				}
			}
			else if (num != 3996115294u)
			{
				if (num == 4088464520u)
				{
					if (typeName == "sbyte")
					{
						return typeof(sbyte);
					}
				}
			}
			else if (typeName == "short?")
			{
				return typeof(short?);
			}
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type = assembly.GetType(typeName, false, true);
				if (type != null)
				{
					return type;
				}
			}
			Type type2 = Type.GetType(typeName, false, true);
			if (type2 != null)
			{
				return type2;
			}
			return null;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			if (type.IsGenericType)
			{
				return type.ToString();
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				if (type.Namespace == GenTypes.IgnoredNamespaceNames[i])
				{
					return type.Name;
				}
			}
			return type.FullName;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000BB50 File Offset: 0x00009D50
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}

		// Token: 0x04000059 RID: 89
		public static readonly List<string> IgnoredNamespaceNames = new List<string>
		{
			"RimWorld",
			"Verse",
			"Verse.AI",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen",
			"RimWorld.QuestGen",
			"RimWorld.SketchGen",
			"System"
		};

		// Token: 0x0400005A RID: 90
		private static Dictionary<GenTypes.TypeCacheKey, Type> typeCache = new Dictionary<GenTypes.TypeCacheKey, Type>(EqualityComparer<GenTypes.TypeCacheKey>.Default);

		// Token: 0x020012F9 RID: 4857
		private struct TypeCacheKey : IEquatable<GenTypes.TypeCacheKey>
		{
			// Token: 0x06007347 RID: 29511 RVA: 0x00281B93 File Offset: 0x0027FD93
			public override int GetHashCode()
			{
				if (this.namespaceIfAmbiguous == null)
				{
					return this.typeName.GetHashCode();
				}
				return (17 * 31 + this.typeName.GetHashCode()) * 31 + this.namespaceIfAmbiguous.GetHashCode();
			}

			// Token: 0x06007348 RID: 29512 RVA: 0x00281BC9 File Offset: 0x0027FDC9
			public bool Equals(GenTypes.TypeCacheKey other)
			{
				return string.Equals(this.typeName, other.typeName) && string.Equals(this.namespaceIfAmbiguous, other.namespaceIfAmbiguous);
			}

			// Token: 0x06007349 RID: 29513 RVA: 0x00281BF1 File Offset: 0x0027FDF1
			public override bool Equals(object obj)
			{
				return obj is GenTypes.TypeCacheKey && this.Equals((GenTypes.TypeCacheKey)obj);
			}

			// Token: 0x0600734A RID: 29514 RVA: 0x00281C09 File Offset: 0x0027FE09
			public TypeCacheKey(string typeName, string namespaceIfAmbigous = null)
			{
				this.typeName = typeName;
				this.namespaceIfAmbiguous = namespaceIfAmbigous;
			}

			// Token: 0x040047E1 RID: 18401
			public string typeName;

			// Token: 0x040047E2 RID: 18402
			public string namespaceIfAmbiguous;
		}
	}
}
