using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107F RID: 4223
	public static class ThingRequiringRoyalPermissionUtility
	{
		// Token: 0x06006437 RID: 25655 RVA: 0x0022B6D0 File Offset: 0x002298D0
		public static bool IsViolatingRulesOf(Def implantOrWeapon, Pawn pawn, Faction faction, int implantLevel = 0)
		{
			if (faction.def.royalImplantRules == null || faction.def.royalImplantRules.Count == 0)
			{
				return false;
			}
			RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, faction, implantLevel);
			if (minTitleToUse == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			if (currentTitle == null)
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num < 0)
			{
				return false;
			}
			int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleToUse);
			return num < num2;
		}

		// Token: 0x06006438 RID: 25656 RVA: 0x0022B74C File Offset: 0x0022994C
		public static bool IsViolatingRulesOfAnyFaction(Def implantOrWeapon, Pawn pawn, int implantLevel = 0)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(implantOrWeapon, pawn, faction, implantLevel))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x0022B7A8 File Offset: 0x002299A8
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static bool IsViolatingRulesOfAnyFaction_NewTemp(Def implantOrWeapon, Pawn pawn, int implantLevel = 0, bool ignoreSilencer = false)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOfAnyFaction(implantOrWeapon, pawn, implantLevel);
		}

		// Token: 0x0600643A RID: 25658 RVA: 0x0022B7B4 File Offset: 0x002299B4
		public static RoyalTitleDef GetMinTitleToUse(Def implantOrWeapon, Faction faction, int implantLevel = 0)
		{
			HediffDef implantDef;
			if ((implantDef = (implantOrWeapon as HediffDef)) != null)
			{
				return faction.GetMinTitleForImplant(implantDef, implantLevel);
			}
			return null;
		}

		// Token: 0x0600643B RID: 25659 RVA: 0x001C39FF File Offset: 0x001C1BFF
		[Obsolete("Will be removed in the future")]
		public static TaggedString GetEquipWeaponConfirmationDialogText(Thing weapon, Pawn pawn)
		{
			return null;
		}
	}
}
