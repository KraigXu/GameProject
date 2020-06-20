using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D0F RID: 3343
	public static class ForbidUtility
	{
		// Token: 0x0600514C RID: 20812 RVA: 0x001B43F4 File Offset: 0x001B25F4
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.", false);
				}
				return;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t, false);
				}
				return;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			if (comp == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + t, false);
				}
				return;
			}
			comp.Forbidden = value;
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001B4458 File Offset: 0x001B2658
		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + t, false);
			}
			if (t.Position.InBounds(t.Map) && !t.Map.areaManager.Home[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001B44B8 File Offset: 0x001B26B8
		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget)
		{
			return (pawn.HostFaction == null || (pawn.HostFaction == Faction.OfPlayer && pawn.Spawned && !pawn.Map.IsPlayerHome && (pawn.GetRoom(RegionType.Set_Passable) == null || !pawn.GetRoom(RegionType.Set_Passable).isPrisonCell) && (!pawn.IsPrisoner || pawn.guest.PrisonerIsSecure))) && !pawn.InMentalState && (!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn));
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001B453C File Offset: 0x001B273C
		public static bool InAllowedArea(this IntVec3 c, Pawn forPawn)
		{
			if (forPawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x001B4578 File Offset: 0x001B2778
		public static bool IsForbidden(this Thing t, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, false))
			{
				return false;
			}
			if (t.Spawned && t.Position.IsForbidden(pawn))
			{
				return true;
			}
			if (t.IsForbidden(pawn.Faction) || t.IsForbidden(pawn.HostFaction))
			{
				return true;
			}
			Lord lord = pawn.GetLord();
			return lord != null && lord.extraForbiddenThings.Contains(t);
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001B45E1 File Offset: 0x001B27E1
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x001B4600 File Offset: 0x001B2800
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x001B4654 File Offset: 0x001B2854
		public static bool IsForbiddenEntirely(this Region r, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				return false;
			}
			if (pawn.playerSettings != null)
			{
				Area effectiveAreaRestriction = pawn.playerSettings.EffectiveAreaRestriction;
				if (effectiveAreaRestriction != null && effectiveAreaRestriction.TrueCount > 0 && effectiveAreaRestriction.Map == r.Map && r.OverlapWith(effectiveAreaRestriction) == AreaOverlap.None)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x001B46A8 File Offset: 0x001B28A8
		public static bool IsForbidden(this Thing t, Faction faction)
		{
			if (faction == null)
			{
				return false;
			}
			if (faction != Faction.OfPlayer)
			{
				return false;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			return comp != null && comp.Forbidden;
		}
	}
}
