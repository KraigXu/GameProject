using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GenerateSite : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests && this.sitePartsParams.GetValue(slate) != null)
			{
				using (IEnumerator<SitePartDefWithParams> enumerator = this.sitePartsParams.GetValue(slate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.wantsThreatPoints)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<SitePartDefWithParams> enumerable = this.sitePartsParams.GetValue(slate);
			bool flag = false;
			using (IEnumerator<SitePartDefWithParams> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.defaultHidden)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag || this.hiddenSitePartsPossible.GetValue(slate))
			{
				SitePartParams parms = SitePartDefOf.PossibleUnknownThreatMarker.Worker.GenerateDefaultParams(0f, this.tile.GetValue(slate), this.faction.GetValue(slate));
				SitePartDefWithParams val = new SitePartDefWithParams(SitePartDefOf.PossibleUnknownThreatMarker, parms);
				enumerable = enumerable.Concat(Gen.YieldSingle<SitePartDefWithParams>(val));
			}
			Site site = SiteMaker.MakeSite(enumerable, this.tile.GetValue(slate), this.faction.GetValue(slate), true);
			List<Rule> list = new List<Rule>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<string> list2 = new List<string>();
			int num = 0;
			for (int i = 0; i < site.parts.Count; i++)
			{
				List<Rule> list3 = new List<Rule>();
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				site.parts[i].def.Worker.Notify_GeneratedByQuestGen(site.parts[i], QuestGen.slate, list3, dictionary2);
				if (!site.parts[i].hidden)
				{
					if (this.singleSitePartRules.GetValue(slate) != null)
					{
						List<Rule> list4 = new List<Rule>();
						list4.AddRange(list3);
						list4.AddRange(this.singleSitePartRules.GetValue(slate).Rules);
						string text = QuestGenUtility.ResolveLocalText(list4, dictionary2, "root", false);
						list.Add(new Rule_String("sitePart" + num + "_description", text));
						if (!text.NullOrEmpty())
						{
							list2.Add(text);
						}
					}
					for (int j = 0; j < list3.Count; j++)
					{
						Rule rule = list3[j].DeepCopy();
						Rule_String rule_String = rule as Rule_String;
						if (rule_String != null && num != 0)
						{
							rule_String.keyword = string.Concat(new object[]
							{
								"sitePart",
								num,
								"_",
								rule_String.keyword
							});
						}
						list.Add(rule);
					}
					foreach (KeyValuePair<string, string> keyValuePair in dictionary2)
					{
						string text2 = keyValuePair.Key;
						if (num != 0)
						{
							text2 = string.Concat(new object[]
							{
								"sitePart",
								num,
								"_",
								text2
							});
						}
						if (!dictionary.ContainsKey(text2))
						{
							dictionary.Add(text2, keyValuePair.Value);
						}
					}
					num++;
				}
			}
			if (!list2.Any<string>())
			{
				list.Add(new Rule_String("allSitePartsDescriptions", "HiddenOrNoSitePartDescription".Translate()));
				list.Add(new Rule_String("allSitePartsDescriptionsExceptFirst", "HiddenOrNoSitePartDescription".Translate()));
			}
			else
			{
				list.Add(new Rule_String("allSitePartsDescriptions", list2.ToClauseSequence()));
				if (list2.Count >= 2)
				{
					list.Add(new Rule_String("allSitePartsDescriptionsExceptFirst", list2.Skip(1).ToList<string>().ToClauseSequence()));
				}
				else
				{
					list.Add(new Rule_String("allSitePartsDescriptionsExceptFirst", "HiddenOrNoSitePartDescription".Translate()));
				}
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Site>(this.storeAs.GetValue(slate), site, false);
			}
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestNameRules(list);
			QuestGen.AddQuestDescriptionConstants(dictionary);
			QuestGen.AddQuestNameConstants(dictionary);
			QuestGen.AddQuestNameRules(new List<Rule>
			{
				new Rule_String("site_label", site.Label)
			});
		}

		
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<int> tile;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<RulePack> singleSitePartRules;

		
		public SlateRef<bool> hiddenSitePartsPossible;

		
		private const string RootSymbol = "root";
	}
}
