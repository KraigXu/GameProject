using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006B9 RID: 1721
	public static class TrashUtility
	{
		// Token: 0x06002E66 RID: 11878 RVA: 0x00104B78 File Offset: 0x00102D78
		public static bool ShouldTrashPlant(Pawn pawn, Plant p)
		{
			if (!p.sown || p.def.plant.IsTree || !p.FlammableNow || !TrashUtility.CanTrash(pawn, p))
			{
				return false;
			}
			foreach (IntVec3 c in CellRect.CenteredOn(p.Position, 2).ClipInsideMap(p.Map))
			{
				if (c.InBounds(p.Map) && c.ContainsStaticFire(p.Map))
				{
					return false;
				}
			}
			return p.Position.Roofed(p.Map) || p.Map.weatherManager.RainRate <= 0.25f;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x00104C5C File Offset: 0x00102E5C
		public static bool ShouldTrashBuilding(Pawn pawn, Building b, bool attackAllInert = false)
		{
			if (!b.def.useHitPoints || (b.def.building != null && b.def.building.ai_neverTrashThis))
			{
				return false;
			}
			if (pawn.mindState.spawnedByInfestationThingComp && b.GetComp<CompCreatesInfestations>() != null)
			{
				return false;
			}
			if (((b.def.building.isInert || b.def.IsFrame) && !attackAllInert) || b.def.building.isTrap)
			{
				int num = GenLocalDate.HourOfDay(pawn) / 3;
				int specialSeed = b.GetHashCode() * 612361 ^ pawn.GetHashCode() * 391 ^ num * 73427324;
				if (!Rand.ChanceSeeded(0.008f, specialSeed))
				{
					return false;
				}
			}
			if (b.def.building.isTrap)
			{
				return false;
			}
			CompCanBeDormant comp = b.GetComp<CompCanBeDormant>();
			return (comp == null || comp.Awake) && b.Faction != Faction.OfMechanoids && TrashUtility.CanTrash(pawn, b) && pawn.HostileTo(b);
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x00104D68 File Offset: 0x00102F68
		private static bool CanTrash(Pawn pawn, Thing t)
		{
			return pawn.CanReach(t, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn) && !t.IsBurning();
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x00104D88 File Offset: 0x00102F88
		public static Job TrashJob(Pawn pawn, Thing t, bool allowPunchingInert = false)
		{
			if (t is Plant)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Ignite, t);
				TrashUtility.FinalizeTrashJob(job);
				return job;
			}
			if (pawn.equipment != null && Rand.Value < 0.7f)
			{
				foreach (Verb verb in pawn.equipment.AllEquipmentVerbs)
				{
					if (verb.verbProps.ai_IsBuildingDestroyer)
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, t);
						job2.verbToUse = verb;
						TrashUtility.FinalizeTrashJob(job2);
						return job2;
					}
				}
			}
			Job job3;
			if (Rand.Value < 0.35f && pawn.natives.IgniteVerb != null && pawn.natives.IgniteVerb.IsStillUsableBy(pawn) && t.FlammableNow && !t.IsBurning() && !(t is Building_Door))
			{
				job3 = JobMaker.MakeJob(JobDefOf.Ignite, t);
			}
			else
			{
				Building building = t as Building;
				if (building != null && building.def.building.isInert && !allowPunchingInert)
				{
					return null;
				}
				job3 = JobMaker.MakeJob(JobDefOf.AttackMelee, t);
			}
			TrashUtility.FinalizeTrashJob(job3);
			return job3;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x00104ED4 File Offset: 0x001030D4
		private static void FinalizeTrashJob(Job job)
		{
			job.expiryInterval = TrashUtility.TrashJobCheckOverrideInterval.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
		}

		// Token: 0x04001A6D RID: 6765
		private const float ChanceHateInertBuilding = 0.008f;

		// Token: 0x04001A6E RID: 6766
		private static readonly IntRange TrashJobCheckOverrideInterval = new IntRange(450, 500);
	}
}
