using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF7 RID: 3575
	public class Alert_RoyalNoAcceptableFood : Alert
	{
		// Token: 0x0600568D RID: 22157 RVA: 0x001CB29D File Offset: 0x001C949D
		public Alert_RoyalNoAcceptableFood()
		{
			this.defaultLabel = "RoyalNoAcceptableFood".Translate();
			this.defaultExplanation = "RoyalNoAcceptableFoodDesc".Translate();
		}

		// Token: 0x17000F71 RID: 3953
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

		// Token: 0x0600568F RID: 22159 RVA: 0x001CB3E8 File Offset: 0x001C95E8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x001CB3F8 File Offset: 0x001C95F8
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

		// Token: 0x04002F37 RID: 12087
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
