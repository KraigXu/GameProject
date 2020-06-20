using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public static class StoragePriorityHelper
	{
		// Token: 0x06004C1C RID: 19484 RVA: 0x001991C4 File Offset: 0x001973C4
		public static string Label(this StoragePriority p)
		{
			switch (p)
			{
			case StoragePriority.Unstored:
				return "StoragePriorityUnstored".Translate();
			case StoragePriority.Low:
				return "StoragePriorityLow".Translate();
			case StoragePriority.Normal:
				return "StoragePriorityNormal".Translate();
			case StoragePriority.Preferred:
				return "StoragePriorityPreferred".Translate();
			case StoragePriority.Important:
				return "StoragePriorityImportant".Translate();
			case StoragePriority.Critical:
				return "StoragePriorityCritical".Translate();
			default:
				return "Unknown";
			}
		}
	}
}
