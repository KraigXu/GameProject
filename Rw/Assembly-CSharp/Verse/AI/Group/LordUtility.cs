using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020005DC RID: 1500
	public static class LordUtility
	{
		// Token: 0x060029BF RID: 10687 RVA: 0x000F513C File Offset: 0x000F333C
		public static Lord GetLord(this Pawn p)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(p);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000F517C File Offset: 0x000F337C
		public static Lord GetLord(this Building b)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(b);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}
	}
}
