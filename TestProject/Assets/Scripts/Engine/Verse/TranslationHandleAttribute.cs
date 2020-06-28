using System;

namespace Verse
{
	// Token: 0x02000369 RID: 873
	[AttributeUsage(AttributeTargets.Field)]
	public class TranslationHandleAttribute : Attribute
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x000A186B File Offset: 0x0009FA6B
		// (set) Token: 0x06001A39 RID: 6713 RVA: 0x000A1873 File Offset: 0x0009FA73
		public int Priority { get; set; }
	}
}
