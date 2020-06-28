using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C1 RID: 1729
	public class JobGiver_PickUpOpportunisticWeapon : ThinkNode_JobGiver
	{
		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002E80 RID: 11904 RVA: 0x001056E0 File Offset: 0x001038E0
		private float MinMeleeWeaponDPSThreshold
		{
			get
			{
				List<Tool> tools = ThingDefOf.Human.tools;
				float num = 0f;
				for (int i = 0; i < tools.Count; i++)
				{
					if (tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.LeftHand || tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.RightHand)
					{
						num = tools[i].power / tools[i].cooldownTime;
						break;
					}
				}
				return num + 2f;
			}
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x00105758 File Offset: 0x00103958
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PickUpOpportunisticWeapon jobGiver_PickUpOpportunisticWeapon = (JobGiver_PickUpOpportunisticWeapon)base.DeepCopy(resolve);
			jobGiver_PickUpOpportunisticWeapon.preferBuildingDestroyers = this.preferBuildingDestroyers;
			return jobGiver_PickUpOpportunisticWeapon;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x00105774 File Offset: 0x00103974
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment == null)
			{
				return null;
			}
			if (this.AlreadySatisfiedWithCurrentWeapon(pawn))
			{
				return null;
			}
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			if (pawn.GetRegion(RegionType.Set_Passable) == null)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquip(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.Equip, thing);
			}
			return null;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x00105864 File Offset: 0x00103A64
		private bool AlreadySatisfiedWithCurrentWeapon(Pawn pawn)
		{
			ThingWithComps primary = pawn.equipment.Primary;
			if (primary == null)
			{
				return false;
			}
			if (this.preferBuildingDestroyers)
			{
				if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
				{
					return false;
				}
			}
			else if (!primary.def.IsRangedWeapon)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x001058B8 File Offset: 0x00103AB8
		private bool ShouldEquip(Thing newWep, Pawn pawn)
		{
			return EquipmentUtility.CanEquip(newWep, pawn) && this.GetWeaponScore(newWep) > this.GetWeaponScore(pawn.equipment.Primary);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x001058E0 File Offset: 0x00103AE0
		private int GetWeaponScore(Thing wep)
		{
			if (wep == null)
			{
				return 0;
			}
			if (wep.def.IsMeleeWeapon && wep.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true) < this.MinMeleeWeaponDPSThreshold)
			{
				return 0;
			}
			if (this.preferBuildingDestroyers && wep.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return 3;
			}
			if (wep.def.IsRangedWeapon)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x04001A72 RID: 6770
		private bool preferBuildingDestroyers;
	}
}
