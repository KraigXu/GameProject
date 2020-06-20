using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001048 RID: 4168
	public static class RoyalTitleDefExt
	{
		// Token: 0x06006391 RID: 25489 RVA: 0x00228C1C File Offset: 0x00226E1C
		public static RoyalTitleDef GetNextTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num == -1 && currentTitle != null)
			{
				return null;
			}
			int num2 = (currentTitle == null) ? 0 : (num + 1);
			if (faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count <= num2)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num2];
		}

		// Token: 0x06006392 RID: 25490 RVA: 0x00228C74 File Offset: 0x00226E74
		public static RoyalTitleDef GetPreviousTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num];
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x00228CC4 File Offset: 0x00226EC4
		public static bool TryInherit(this RoyalTitleDef title, Pawn from, Faction faction, out RoyalTitleInheritanceOutcome outcome)
		{
			outcome = default(RoyalTitleInheritanceOutcome);
			if (title.GetInheritanceWorker(faction) == null)
			{
				return false;
			}
			Pawn heir = from.royalty.GetHeir(faction);
			if (heir == null || heir.Destroyed)
			{
				return false;
			}
			RoyalTitleDef currentTitle = heir.royalty.GetCurrentTitle(faction);
			bool heirTitleHigher = currentTitle != null && currentTitle.seniority >= title.seniority;
			outcome = new RoyalTitleInheritanceOutcome
			{
				heir = heir,
				heirCurrentTitle = currentTitle,
				heirTitleHigher = heirTitleHigher
			};
			return true;
		}
	}
}
