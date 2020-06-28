using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000495 RID: 1173
	internal static class WorkshopUtility
	{
		// Token: 0x060022C7 RID: 8903 RVA: 0x000D3220 File Offset: 0x000D1420
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			if (stage == WorkshopInteractStage.None)
			{
				return "None".Translate();
			}
			return ("WorkshopInteractStage_" + stage.ToString()).Translate();
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000D3256 File Offset: 0x000D1456
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000D3279 File Offset: 0x000D1479
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}
