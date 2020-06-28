using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000A44 RID: 2628
	public static class ResolveParamsUtility
	{
		// Token: 0x06003E25 RID: 15909 RVA: 0x0014750C File Offset: 0x0014570C
		public static void SetCustom<T>(ref Dictionary<string, object> custom, string name, T obj, bool inherit = false)
		{
			if (custom == null)
			{
				custom = new Dictionary<string, object>();
			}
			else
			{
				custom = new Dictionary<string, object>(custom);
			}
			if (!custom.ContainsKey(name))
			{
				custom.Add(name, obj);
				return;
			}
			if (!inherit)
			{
				custom[name] = obj;
			}
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x00147559 File Offset: 0x00145759
		public static void RemoveCustom(ref Dictionary<string, object> custom, string name)
		{
			if (custom == null)
			{
				return;
			}
			custom = new Dictionary<string, object>(custom);
			custom.Remove(name);
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x00147574 File Offset: 0x00145774
		public static bool TryGetCustom<T>(Dictionary<string, object> custom, string name, out T obj)
		{
			object obj2;
			if (custom == null || !custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				return false;
			}
			obj = (T)((object)obj2);
			return true;
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x001475A8 File Offset: 0x001457A8
		public static T GetCustom<T>(Dictionary<string, object> custom, string name)
		{
			object obj;
			if (custom == null || !custom.TryGetValue(name, out obj))
			{
				return default(T);
			}
			return (T)((object)obj);
		}
	}
}
