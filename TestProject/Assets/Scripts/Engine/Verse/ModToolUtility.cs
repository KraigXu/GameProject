using System;

namespace Verse
{
	// Token: 0x02000378 RID: 888
	public static class ModToolUtility
	{
		// Token: 0x06001A5D RID: 6749 RVA: 0x000A21E7 File Offset: 0x000A03E7
		public static bool IsValueEditable(this Type type)
		{
			return type.IsValueType || type == typeof(string);
		}
	}
}
