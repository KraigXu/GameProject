using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	
	public static class GenTypes
	{
		
		
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
				}
				yield break;
			}
		}

		
		
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

		
		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return from x in GenTypes.AllTypes
			where x.HasAttribute<TAttr>()
			select x;
		}

		
		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		
		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		
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

		
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			if (typeName == "byte?")
			{
				return typeof(byte?);
			}

			if (typeName == "string")
			{
				return typeof(string);
			}

			if (typeName == "char?")
			{
				return typeof(char?);
			}

			if (typeName == "float?")
			{
				return typeof(float?);
			}

			if (typeName == "decimal")
			{
				return typeof(decimal);
			}

			if (typeName == "uint?")
			{
				return typeof(uint?);
			}

			if (typeName == "sbyte?")
			{
				return typeof(sbyte?);
			}

			if (typeName == "decimal?")
			{
				return typeof(decimal?);
			}

			if (typeName == "long?")
			{
				return typeof(long?);
			}

			if (typeName == "ushort")
			{
				return typeof(ushort);
			}

			if (typeName == "int?")
			{
				return typeof(int?);
			}
			if (typeName == "double?")
			{
				return typeof(double?);
			}

			if (typeName == "byte")
			{
				return typeof(byte);
			}
			if (typeName == "int")
			{
				return typeof(int);
			}
			if (typeName == "ulong?")
			{
				return typeof(ulong?);
			}
			if (typeName == "ushort?")
			{
				return typeof(ushort?);
			}
			if (typeName == "double")
			{
				return typeof(double);
			}

			if (typeName == "ulong")
			{
				return typeof(ulong);
			}
			if (typeName == "char")
			{
				return typeof(char);
			}
			if (typeName == "float")
			{
				return typeof(float);
			}
			if (typeName == "bool?")
			{
				return typeof(bool?);
			}

			if (typeName == "long")
			{
				return typeof(long);
			}

			if (typeName == "short")
			{
				return typeof(short);
			}

			if (typeName == "uint")
			{
				return typeof(uint);
			}

			if (typeName == "bool")
			{
				return typeof(bool);
			}

			if (typeName == "sbyte")
			{
				return typeof(sbyte);
			}

			if (typeName == "short?")
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

		
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}

		
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

		
		private static Dictionary<GenTypes.TypeCacheKey, Type> typeCache = new Dictionary<GenTypes.TypeCacheKey, Type>(EqualityComparer<GenTypes.TypeCacheKey>.Default);

		
		private struct TypeCacheKey : IEquatable<GenTypes.TypeCacheKey>
		{
			
			public override int GetHashCode()
			{
				if (this.namespaceIfAmbiguous == null)
				{
					return this.typeName.GetHashCode();
				}
				return (17 * 31 + this.typeName.GetHashCode()) * 31 + this.namespaceIfAmbiguous.GetHashCode();
			}

			
			public bool Equals(GenTypes.TypeCacheKey other)
			{
				return string.Equals(this.typeName, other.typeName) && string.Equals(this.namespaceIfAmbiguous, other.namespaceIfAmbiguous);
			}

			
			public override bool Equals(object obj)
			{
				return obj is GenTypes.TypeCacheKey && this.Equals((GenTypes.TypeCacheKey)obj);
			}

			
			public TypeCacheKey(string typeName, string namespaceIfAmbigous = null)
			{
				this.typeName = typeName;
				this.namespaceIfAmbiguous = namespaceIfAmbigous;
			}

			
			public string typeName;

			
			public string namespaceIfAmbiguous;
		}
	}
}
