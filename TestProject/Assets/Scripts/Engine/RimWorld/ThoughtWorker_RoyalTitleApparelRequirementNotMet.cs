using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000849 RID: 2121
	public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
	{
		// Token: 0x060034A3 RID: 13475 RVA: 0x00120808 File Offset: 0x0011EA08
		private static RoyalTitleDef Validate(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				return null;
			}
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				if (royalTitle.def.requiredApparel != null && royalTitle.def.requiredApparel.Count > 0)
				{
					for (int i = 0; i < royalTitle.def.requiredApparel.Count; i++)
					{
						if (!royalTitle.def.requiredApparel[i].IsMet(p))
						{
							return royalTitle.def;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x001208D0 File Offset: 0x0011EAD0
		private static IEnumerable<string> GetFirstRequiredApparelPerGroup(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				yield break;
			}
			foreach (RoyalTitle t in p.royalty.AllTitlesInEffectForReading)
			{
				if (t.def.requiredApparel != null && t.def.requiredApparel.Count > 0)
				{
					int num;
					for (int i = 0; i < t.def.requiredApparel.Count; i = num + 1)
					{
						RoyalTitleDef.ApparelRequirement apparelRequirement = t.def.requiredApparel[i];
						if (!apparelRequirement.IsMet(p))
						{
							yield return apparelRequirement.AllRequiredApparelForPawn(p, false, false).First<ThingDef>().LabelCap;
						}
						num = i;
					}
				}
				t = null;
			}
			List<RoyalTitle>.Enumerator enumerator = default(List<RoyalTitle>.Enumerator);
			yield return "ApparelRequirementAnyPrestigeArmor".Translate();
			yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			yield break;
			yield break;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x001208E0 File Offset: 0x0011EAE0
		public override string PostProcessLabel(Pawn p, string label)
		{
			RoyalTitleDef royalTitleDef = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitleDef == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitleDef.GetLabelCapFor(p).Named("TITLE"), p.Named("PAWN"));
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00120924 File Offset: 0x0011EB24
		public override string PostProcessDescription(Pawn p, string description)
		{
			RoyalTitleDef royalTitleDef = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitleDef == null)
			{
				return string.Empty;
			}
			return description.Formatted(ThoughtWorker_RoyalTitleApparelRequirementNotMet.GetFirstRequiredApparelPerGroup(p).ToLineList("- ", false), royalTitleDef.GetLabelCapFor(p).Named("TITLE"), p.Named("PAWN"));
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x0012097E File Offset: 0x0011EB7E
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
