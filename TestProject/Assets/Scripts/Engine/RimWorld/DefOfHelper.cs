using System;
using System.Reflection;
using Verse;

namespace RimWorld
{
	
	public static class DefOfHelper
	{
		private static bool bindingNow;
		private static bool earlyTry = true;


		public static void RebindAllDefOfs(bool earlyTryMode)
		{
			DefOfHelper.earlyTry = earlyTryMode;
			DefOfHelper.bindingNow = true;
			try
			{
				foreach (Type type in GenTypes.AllTypesWithAttribute<DefOf>())
				{
					DefOfHelper.BindDefsFor(type);
				}
			}
			finally
			{
				DefOfHelper.bindingNow = false;
			}
		}

		
		private static void BindDefsFor(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				Type fieldType = fieldInfo.FieldType;
				if (!typeof(Def).IsAssignableFrom(fieldType))
				{
					Log.Error(fieldType + " is not a Def.", false);
				}
				else
				{
					MayRequireAttribute mayRequireAttribute = fieldInfo.TryGetAttribute<MayRequireAttribute>();
					bool flag = (mayRequireAttribute == null || ModsConfig.IsActive(mayRequireAttribute.modId)) && !DefOfHelper.earlyTry;
					string text = fieldInfo.Name;
					DefAliasAttribute defAliasAttribute = fieldInfo.TryGetAttribute<DefAliasAttribute>();
					if (defAliasAttribute != null)
					{
						text = defAliasAttribute.defName;
					}
					if (fieldType == typeof(SoundDef))
					{
						SoundDef soundDef = SoundDef.Named(text);
						if (soundDef.isUndefined && flag)
						{
							Log.Error("Could not find SoundDef named " + text, false);
						}
						fieldInfo.SetValue(null, soundDef);
					}
					else
					{
						Def def = GenDefDatabase.GetDef(fieldType, text, flag);
						fieldInfo.SetValue(null, def);
					}
				}
			}
		}

		
		public static void EnsureInitializedInCtor(Type defOf)
		{
			if (!DefOfHelper.bindingNow)
			{
				string text;
				if (DirectXmlToObject.currentlyInstantiatingObjectOfType.Any<Type>())
				{
					text = "DirectXmlToObject is currently instantiating an object of type " + DirectXmlToObject.currentlyInstantiatingObjectOfType.Peek();
				}
				else if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					text = "curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent;
				}
				else
				{
					text = "";
				}
				Log.Warning("Tried to use an uninitialized DefOf of type " + defOf.Name + ". DefOfs are initialized right after all defs all loaded. Uninitialized DefOfs will return only nulls. (hint: don't use DefOfs as default field values in Defs, try to resolve them in ResolveReferences() instead)" + (text.NullOrEmpty() ? "" : (" Debug info: " + text)), false);
			}
		}

		

	}
}
