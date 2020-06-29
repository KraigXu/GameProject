using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
	{
		
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

		
		public override string PostProcessLabel(Pawn p, string label)
		{
			RoyalTitleDef royalTitleDef = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitleDef == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitleDef.GetLabelCapFor(p).Named("TITLE"), p.Named("PAWN"));
		}

		
		public override string PostProcessDescription(Pawn p, string description)
		{
			RoyalTitleDef royalTitleDef = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitleDef == null)
			{
				return string.Empty;
			}
			return description.Formatted(ThoughtWorker_RoyalTitleApparelRequirementNotMet.GetFirstRequiredApparelPerGroup(p).ToLineList("- ", false), royalTitleDef.GetLabelCapFor(p).Named("TITLE"), p.Named("PAWN"));
		}

		
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
