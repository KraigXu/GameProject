using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC8 RID: 4040
	public static class InventoryCalculatorsUtility
	{
		// Token: 0x06006110 RID: 24848 RVA: 0x0021B060 File Offset: 0x00219260
		public static bool ShouldIgnoreInventoryOf(Pawn pawn, IgnorePawnsInventoryMode ignoreMode)
		{
			switch (ignoreMode)
			{
			case IgnorePawnsInventoryMode.Ignore:
				return true;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload:
				return pawn.Spawned && pawn.inventory.UnloadEverything;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn:
				return (pawn.Spawned && pawn.inventory.UnloadEverything) || Dialog_FormCaravan.CanListInventorySeparately(pawn);
			case IgnorePawnsInventoryMode.DontIgnore:
				return false;
			default:
				throw new NotImplementedException("IgnorePawnsInventoryMode");
			}
		}
	}
}
