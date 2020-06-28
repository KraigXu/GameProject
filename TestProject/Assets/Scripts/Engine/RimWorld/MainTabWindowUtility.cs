using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008E2 RID: 2274
	public static class MainTabWindowUtility
	{
		// Token: 0x06003678 RID: 13944 RVA: 0x001271D8 File Offset: 0x001253D8
		public static void NotifyAllPawnTables_PawnsChanged()
		{
			if (Find.WindowStack == null)
			{
				return;
			}
			WindowStack windowStack = Find.WindowStack;
			for (int i = 0; i < windowStack.Count; i++)
			{
				MainTabWindow_PawnTable mainTabWindow_PawnTable = windowStack[i] as MainTabWindow_PawnTable;
				if (mainTabWindow_PawnTable != null)
				{
					mainTabWindow_PawnTable.Notify_PawnsChanged();
				}
			}
		}
	}
}
