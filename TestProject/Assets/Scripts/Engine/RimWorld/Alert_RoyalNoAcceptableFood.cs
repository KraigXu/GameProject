using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Alert_RoyalNoAcceptableFood : Alert
	{
		
		public Alert_RoyalNoAcceptableFood()
		{
			this.defaultLabel = "RoyalNoAcceptableFood".Translate();
			this.defaultExplanation = "RoyalNoAcceptableFoodDesc".Translate();
		}

		
		// (get) Token: 0x0600568E RID: 22158 RVA: 0x001CB2DC File Offset: 0x001C94DC
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (pawn.Spawned && (pawn.story == null || !pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
						{
							Pawn_RoyaltyTracker royalty = pawn.royalty;
							RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
							Thing thing;
							ThingDef thingDef;
							if (royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined && !FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, false, out thing, out thingDef, true, true, false, false, false, false, false, true, FoodPreferability.DesperateOnly))
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.Targets.Select(delegate(Pawn t)
			{
				RoyalTitle mostSeniorTitle = t.royalty.MostSeniorTitle;
				string[] array = new string[5];
				array[0] = t.LabelShort;
				array[1] = " (";
				array[2] = mostSeniorTitle.def.GetLabelFor(t.gender);
				array[3] = "):\n";
				array[4] = (from m in mostSeniorTitle.def.SatisfyingMeals(false)
				select m.LabelCap).ToLineList("- ", false);
				return string.Concat(array);
			}).ToLineList("\n", false);
		}

		
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
