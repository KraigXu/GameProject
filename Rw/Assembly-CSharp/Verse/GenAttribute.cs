using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x0200001A RID: 26
	public static class GenAttribute
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00008104 File Offset: 0x00006304
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T t;
			return memberInfo.TryGetAttribute(out t);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000811C File Offset: 0x0000631C
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

		// Token: 0x060001B6 RID: 438 RVA: 0x00008178 File Offset: 0x00006378
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result = default(T);
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
