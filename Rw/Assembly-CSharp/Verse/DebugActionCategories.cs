using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000356 RID: 854
	public static class DebugActionCategories
	{
		// Token: 0x06001A03 RID: 6659 RVA: 0x0009FE44 File Offset: 0x0009E044
		static DebugActionCategories()
		{
			DebugActionCategories.categoryOrders.Add("Incidents", 100);
			DebugActionCategories.categoryOrders.Add("Quests", 200);
			DebugActionCategories.categoryOrders.Add("Quests (old)", 250);
			DebugActionCategories.categoryOrders.Add("Translation", 300);
			DebugActionCategories.categoryOrders.Add("General", 400);
			DebugActionCategories.categoryOrders.Add("Pawns", 500);
			DebugActionCategories.categoryOrders.Add("Spawning", 600);
			DebugActionCategories.categoryOrders.Add("Map management", 700);
			DebugActionCategories.categoryOrders.Add("Autotests", 800);
			DebugActionCategories.categoryOrders.Add("Mods", 900);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0009FF20 File Offset: 0x0009E120
		public static int GetOrderFor(string category)
		{
			int result;
			if (DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		// Token: 0x04000F24 RID: 3876
		public const string Incidents = "Incidents";

		// Token: 0x04000F25 RID: 3877
		public const string Quests = "Quests";

		// Token: 0x04000F26 RID: 3878
		public const string QuestsOld = "Quests (old)";

		// Token: 0x04000F27 RID: 3879
		public const string Translation = "Translation";

		// Token: 0x04000F28 RID: 3880
		public const string General = "General";

		// Token: 0x04000F29 RID: 3881
		public const string Pawns = "Pawns";

		// Token: 0x04000F2A RID: 3882
		public const string Spawning = "Spawning";

		// Token: 0x04000F2B RID: 3883
		public const string MapManagement = "Map management";

		// Token: 0x04000F2C RID: 3884
		public const string Autotests = "Autotests";

		// Token: 0x04000F2D RID: 3885
		public const string Mods = "Mods";

		// Token: 0x04000F2E RID: 3886
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
