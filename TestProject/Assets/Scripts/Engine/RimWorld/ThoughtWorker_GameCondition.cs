using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000830 RID: 2096
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x0600346F RID: 13423 RVA: 0x0011FD88 File Offset: 0x0011DF88
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.SpawnedOrAnyParentSpawned && p.MapHeld.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			if (Find.World.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			return false;
		}
	}
}
