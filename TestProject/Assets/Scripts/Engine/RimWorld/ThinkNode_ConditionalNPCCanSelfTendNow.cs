using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007F7 RID: 2039
	public class ThinkNode_ConditionalNPCCanSelfTendNow : ThinkNode_Conditional
	{
		// Token: 0x060033E5 RID: 13285 RVA: 0x0011E0D4 File Offset: 0x0011C2D4
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.health.hediffSet.hediffs.Any<Hediff>())
			{
				return false;
			}
			if (!pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner)
			{
				return false;
			}
			if (pawn.InBed() && pawn.guest != null && pawn.Faction != pawn.guest.HostFaction)
			{
				Building_Bed building_Bed = pawn.CurrentBed();
				if (building_Bed.Medical && building_Bed.Faction == pawn.guest.HostFaction)
				{
					return false;
				}
			}
			if (Find.TickManager.TicksGame < pawn.mindState.lastHarmTick + 300)
			{
				return false;
			}
			if (Find.TickManager.TicksGame < pawn.mindState.lastEngageTargetTick + 300)
			{
				return false;
			}
			if (Find.TickManager.TicksGame < pawn.mindState.lastSelfTendTick + 300)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil != null && !lord.CurLordToil.AllowSelfTend)
			{
				return false;
			}
			if (!pawn.health.HasHediffsNeedingTend(false))
			{
				return false;
			}
			if (pawn.Faction != null)
			{
				bool foundActiveThreat = false;
				RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, RegionTraverser.PassAll, delegate(Region x)
				{
					List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
					for (int i = 0; i < list.Count; i++)
					{
						if (GenHostility.IsActiveThreatTo((IAttackTarget)list[i], pawn.Faction))
						{
							foundActiveThreat = true;
							break;
						}
					}
					return foundActiveThreat;
				}, 5, RegionType.Set_Passable);
				if (foundActiveThreat)
				{
					return false;
				}
			}
			return true;
		}
	}
}
