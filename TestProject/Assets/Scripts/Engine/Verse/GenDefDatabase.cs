using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Verse
{
	
	public static class GenDefDatabase
	{
		
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		
		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{

			Def v = null;

			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				v= SoundDef.Named(targetDefName);
			}


			v= (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail", new object[]
			{
				targetDefName
			});


			return v;
		}

		
		public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
		{
			return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
		}

		
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			foreach (Type type in typeof(Def).AllSubclasses())
			{
				if (!type.IsAbstract && !(type == typeof(Def)))
				{
					bool flag = false;
					Type baseType = type.BaseType;
					while (baseType != null && baseType != typeof(Def))
					{
						if (!baseType.IsAbstract)
						{
							flag = true;
							break;
						}
						baseType = baseType.BaseType;
					}
					if (!flag)
					{
						yield return type;
					}
				}
			}
			IEnumerator<Type> enumerator = null;
			yield break;
			yield break;
		}

		
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}
	}
}
