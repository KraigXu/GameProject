using System;

namespace Verse
{
	// Token: 0x02000129 RID: 297
	public static class NamedArgumentUtility
	{
		// Token: 0x0600086F RID: 2159 RVA: 0x00029A69 File Offset: 0x00027C69
		public static NamedArgument Named(this object arg, string label)
		{
			return new NamedArgument(arg, label);
		}
	}
}
