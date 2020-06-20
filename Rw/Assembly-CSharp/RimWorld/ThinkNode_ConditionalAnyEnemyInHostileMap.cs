using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F6 RID: 2038
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		// Token: 0x060033E3 RID: 13283 RVA: 0x0011E088 File Offset: 0x0011C288
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			Map map = pawn.Map;
			return !map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map, true);
		}
	}
}
