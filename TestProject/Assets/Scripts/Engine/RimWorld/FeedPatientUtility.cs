﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073F RID: 1855
	public static class FeedPatientUtility
	{
		// Token: 0x060030AD RID: 12461 RVA: 0x00111340 File Offset: 0x0010F540
		public static bool ShouldBeFed(Pawn p)
		{
			if (p.GetPosture() == PawnPosture.Standing)
			{
				return false;
			}
			if (p.NonHumanlikeOrWildMan())
			{
				Building_Bed building_Bed = p.CurrentBed();
				if (building_Bed == null || building_Bed.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (p.Faction != Faction.OfPlayer && p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (!p.InBed())
				{
					return false;
				}
			}
			if (!p.RaceProps.EatsFood)
			{
				return false;
			}
			if (p.Spawned && p.Map.designationManager.DesignationOn(p, DesignationDefOf.Slaughter) != null)
			{
				return false;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(p))
			{
				return false;
			}
			if (p.HostFaction != null)
			{
				if (p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (p.guest != null && !p.guest.CanBeBroughtFood)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x00111408 File Offset: 0x0010F608
		public static bool IsHungry(Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.CurLevelPercentage <= p.needs.food.PercentageThreshHungry + 0.02f;
		}
	}
}
