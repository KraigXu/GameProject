using System;
using System.Reflection;

namespace Verse
{
	
	public static class GenAttribute
	{
		
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		
		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), true);
			if (customAttributes.Length == 0)
			{
				customAttribute = default(T);
				return false;
			}
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is T)
				{
					customAttribute = (T)((object)customAttributes[i]);
					return true;
				}
			}
			customAttribute = default(T);
			return false;
		}

		
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = default(T);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
