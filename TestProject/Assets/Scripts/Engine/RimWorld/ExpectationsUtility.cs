using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBA RID: 4026
	public static class ExpectationsUtility
	{
		// Token: 0x060060C5 RID: 24773 RVA: 0x00217608 File Offset: 0x00215808
		public static void Reset()
		{
			ExpectationsUtility.wealthExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.WealthTriggered
			orderby ed.order
			select ed).ToList<ExpectationDef>();
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x0021766C File Offset: 0x0021586C
		public static ExpectationDef CurrentExpectationFor(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (p.Faction != Faction.OfPlayer && !p.IsPrisonerOfColony)
			{
				return ExpectationDefOf.ExtremelyLow;
			}
			if (p.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
				if (p.royalty != null && p.MapHeld.IsPlayerHome)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						RoyalTitle currentTitleInFaction = p.royalty.GetCurrentTitleInFaction(faction);
						if (currentTitleInFaction != null && currentTitleInFaction.conceited && currentTitleInFaction.def.minExpectation != null && currentTitleInFaction.def.minExpectation.order > expectationDef.order)
						{
							expectationDef = currentTitleInFaction.def.minExpectation;
						}
					}
				}
				return expectationDef;
			}
			return ExpectationDefOf.VeryLow;
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x00217760 File Offset: 0x00215960
		public static ExpectationDef CurrentExpectationFor(Map m)
		{
			float wealthTotal = m.wealthWatcher.WealthTotal;
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (wealthTotal < expectationDef.maxMapWealth)
				{
					return expectationDef;
				}
			}
			return ExpectationsUtility.wealthExpectationsInOrder[ExpectationsUtility.wealthExpectationsInOrder.Count - 1];
		}

		// Token: 0x04003AFA RID: 15098
		private static List<ExpectationDef> wealthExpectationsInOrder;
	}
}
