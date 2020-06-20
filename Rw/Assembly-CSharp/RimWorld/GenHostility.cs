using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000BF0 RID: 3056
	public static class GenHostility
	{
		// Token: 0x060048AC RID: 18604 RVA: 0x0018B578 File Offset: 0x00189778
		public static bool HostileTo(this Thing a, Thing b)
		{
			if (a.Destroyed || b.Destroyed || a == b)
			{
				return false;
			}
			Pawn pawn = a as Pawn;
			Pawn pawn2 = b as Pawn;
			return (pawn != null && pawn.MentalState != null && pawn.MentalState.ForceHostileTo(b)) || (pawn2 != null && pawn2.MentalState != null && pawn2.MentalState.ForceHostileTo(a)) || (pawn != null && pawn2 != null && (GenHostility.IsPredatorHostileTo(pawn, pawn2) || GenHostility.IsPredatorHostileTo(pawn2, pawn))) || ((a.Faction != null && pawn2 != null && pawn2.HostFaction == a.Faction && (pawn == null || pawn.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn2)) || (b.Faction != null && pawn != null && pawn.HostFaction == b.Faction && (pawn2 == null || pawn2.HostFaction == null) && PrisonBreakUtility.IsPrisonBreaking(pawn))) || ((a.Faction == null || pawn2 == null || pawn2.HostFaction != a.Faction) && (b.Faction == null || pawn == null || pawn.HostFaction != b.Faction) && (pawn == null || !pawn.IsPrisoner || pawn2 == null || !pawn2.IsPrisoner) && (pawn == null || pawn2 == null || ((!pawn.IsPrisoner || pawn.HostFaction != pawn2.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (!pawn2.IsPrisoner || pawn2.HostFaction != pawn.HostFaction || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (pawn == null || pawn2 == null || ((pawn.HostFaction == null || pawn2.Faction == null || pawn.HostFaction.HostileTo(pawn2.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn)) && (pawn2.HostFaction == null || pawn.Faction == null || pawn2.HostFaction.HostileTo(pawn.Faction) || PrisonBreakUtility.IsPrisonBreaking(pawn2)))) && (a.Faction == null || !a.Faction.IsPlayer || pawn2 == null || !pawn2.mindState.WillJoinColonyIfRescued) && (b.Faction == null || !b.Faction.IsPlayer || pawn == null || !pawn.mindState.WillJoinColonyIfRescued) && ((pawn != null && pawn.Faction == null && pawn.RaceProps.Humanlike && b.Faction != null && b.Faction.def.hostileToFactionlessHumanlikes) || (pawn2 != null && pawn2.Faction == null && pawn2.RaceProps.Humanlike && a.Faction != null && a.Faction.def.hostileToFactionlessHumanlikes) || (a.Faction != null && b.Faction != null && a.Faction.HostileTo(b.Faction))));
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0018B814 File Offset: 0x00189A14
		public static bool HostileTo(this Thing t, Faction fac)
		{
			if (t.Destroyed)
			{
				return false;
			}
			if (fac == null)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				MentalState mentalState = pawn.MentalState;
				if (mentalState != null && mentalState.ForceHostileTo(fac))
				{
					return true;
				}
				if (GenHostility.IsPredatorHostileTo(pawn, fac))
				{
					return true;
				}
				if (pawn.HostFaction == fac && PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return true;
				}
				if (pawn.HostFaction == fac)
				{
					return false;
				}
				if (pawn.HostFaction != null && !pawn.HostFaction.HostileTo(fac) && !PrisonBreakUtility.IsPrisonBreaking(pawn))
				{
					return false;
				}
				if (fac.IsPlayer && pawn.mindState.WillJoinColonyIfRescued)
				{
					return false;
				}
				if (fac.def.hostileToFactionlessHumanlikes && pawn.Faction == null && pawn.RaceProps.Humanlike)
				{
					return true;
				}
			}
			return t.Faction != null && t.Faction.HostileTo(fac);
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0018B8EC File Offset: 0x00189AEC
		private static bool IsPredatorHostileTo(Pawn predator, Pawn toPawn)
		{
			if (toPawn.Faction == null)
			{
				return false;
			}
			if (toPawn.Faction.HasPredatorRecentlyAttackedAnyone(predator))
			{
				return true;
			}
			Pawn preyOfMyFaction = GenHostility.GetPreyOfMyFaction(predator, toPawn.Faction);
			return preyOfMyFaction != null && predator.Position.InHorDistOf(preyOfMyFaction.Position, 12f);
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x0018B941 File Offset: 0x00189B41
		private static bool IsPredatorHostileTo(Pawn predator, Faction toFaction)
		{
			return toFaction.HasPredatorRecentlyAttackedAnyone(predator) || GenHostility.GetPreyOfMyFaction(predator, toFaction) != null;
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x0018B95C File Offset: 0x00189B5C
		private static Pawn GetPreyOfMyFaction(Pawn predator, Faction myFaction)
		{
			Job curJob = predator.CurJob;
			if (curJob != null && curJob.def == JobDefOf.PredatorHunt && !predator.jobs.curDriver.ended)
			{
				Pawn pawn = curJob.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn != null && !pawn.Dead && pawn.Faction == myFaction)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x0018B9BE File Offset: 0x00189BBE
		public static bool AnyHostileActiveThreatToPlayer(Map map, bool countDormantPawnsAsHostile = false)
		{
			return GenHostility.AnyHostileActiveThreatTo(map, Faction.OfPlayer, countDormantPawnsAsHostile);
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x0018B9CC File Offset: 0x00189BCC
		public static bool AnyHostileActiveThreatTo(Map map, Faction faction, bool countDormantPawnsAsHostile = false)
		{
			foreach (IAttackTarget attackTarget in map.attackTargetsCache.TargetsHostileToFaction(faction))
			{
				if (GenHostility.IsActiveThreatTo(attackTarget, faction))
				{
					return true;
				}
				Pawn pawn;
				if (countDormantPawnsAsHostile && attackTarget.Thing.HostileTo(faction) && !attackTarget.Thing.Fogged() && !attackTarget.ThreatDisabled(null) && (pawn = (attackTarget.Thing as Pawn)) != null)
				{
					CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
					if (comp != null && !comp.Awake)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x0018BA80 File Offset: 0x00189C80
		public static bool IsActiveThreatToPlayer(IAttackTarget target)
		{
			return GenHostility.IsActiveThreatTo(target, Faction.OfPlayer);
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x0018BA90 File Offset: 0x00189C90
		public static bool IsActiveThreatTo(IAttackTarget target, Faction faction)
		{
			if (!target.Thing.HostileTo(faction))
			{
				return false;
			}
			if (!(target.Thing is IAttackTargetSearcher))
			{
				return false;
			}
			if (target.ThreatDisabled(null))
			{
				return false;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.LordJob is LordJob_DefendAndExpandHive && (pawn.mindState.duty == null || pawn.mindState.duty.def != DutyDefOf.AssaultColony))
				{
					return false;
				}
			}
			Pawn pawn2 = target.Thing as Pawn;
			if (pawn2 != null && (pawn2.MentalStateDef == MentalStateDefOf.PanicFlee || pawn2.IsPrisoner))
			{
				return false;
			}
			CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null && !compCanBeDormant.Awake)
			{
				return false;
			}
			CompInitiatable compInitiatable = target.Thing.TryGetComp<CompInitiatable>();
			if (compInitiatable != null && !compInitiatable.Initiated)
			{
				return false;
			}
			if (target.Thing.Spawned)
			{
				TraverseParms traverseParms = (pawn2 != null) ? TraverseParms.For(pawn2, Danger.Deadly, TraverseMode.ByPawn, false) : TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
				if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x0018BBBA File Offset: 0x00189DBA
		public static bool IsDefMechClusterThreat(ThingDef def)
		{
			return (def.building != null && (def.building.IsTurret || def.building.IsMortar)) || def.isMechClusterThreat;
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x0018BBE6 File Offset: 0x00189DE6
		public static void Notify_PawnLostForTutor(Pawn pawn, Map map)
		{
			if (!map.IsPlayerHome && map.mapPawns.FreeColonistsSpawnedCount != 0 && !GenHostility.AnyHostileActiveThreatToPlayer(map, false))
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ReformCaravan, OpportunityType.Important);
			}
		}
	}
}
