using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GiveRewards : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return this.nodeIfChosenPawnSignalUsed == null || this.nodeIfChosenPawnSignalUsed.TestRun(slate);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn pawn = slate.Get<Pawn>("asker", null, false);
			bool flag = this.useDifficultyFactor.GetValue(slate) ?? true;
			RewardsGeneratorParams value = this.parms.GetValue(slate);
			value.rewardValue = slate.Get<float>("rewardValue", 0f, false);
			if (flag)
			{
				value.rewardValue *= Find.Storyteller.difficulty.questRewardValueFactor;
			}
			if (slate.Get<bool>("debugDontGenerateRewardThings", false, false))
			{
				DebugActionsQuests.lastQuestGeneratedRewardValue += Mathf.Max(value.rewardValue, 250f);
				return;
			}
			value.minGeneratedRewardValue = 250f;
			value.giverFaction = ((pawn != null) ? pawn.Faction : null);
			value.populationIntent = QuestTuning.PopIncreasingRewardWeightByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
			if (value.giverFaction == null || value.giverFaction.def.permanentEnemy)
			{
				value.allowGoodwill = false;
			}
			if (value.giverFaction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction) || value.giverFaction.HostileTo(Faction.OfPlayer))
			{
				value.allowRoyalFavor = false;
			}
			Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo("inSignal");
			if (!this.inSignal.GetValue(slate).NullOrEmpty())
			{
				slate.Set<string>("inSignal", QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)), false);
			}
			try
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = slate.Get<string>("inSignal", null, false);
				bool flag2 = false;
				int num;
				if (value.allowGoodwill && value.giverFaction != null && value.giverFaction.HostileTo(Faction.OfPlayer))
				{
					num = 1;
				}
				else
				{
					num = (this.variants.GetValue(slate) ?? (QuestGen.quest.root.autoAccept ? 1 : 3));
				}
				this.generatedRewards.Clear();
				for (int i = 0; i < num; i++)
				{
					QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
					List<Reward> list = this.GenerateRewards(value, slate, i == 0, ref flag2, choice, num);
					if (list != null)
					{
						questPart_Choice.choices.Add(choice);
						this.generatedRewards.Add(list);
					}
				}
				this.generatedRewards.Clear();
				if (this.addCampLootReward.GetValue(slate))
				{
					for (int j = 0; j < questPart_Choice.choices.Count; j++)
					{
						questPart_Choice.choices[j].rewards.Add(new Reward_CampLoot());
					}
				}
				questPart_Choice.choices.SortByDescending(new Func<QuestPart_Choice.Choice, int>(this.GetDisplayPriority));
				QuestGen.quest.AddPart(questPart_Choice);
				if (flag2 && this.nodeIfChosenPawnSignalUsed != null)
				{
					QuestNode_GiveRewards.tmpPrevQuestParts.Clear();
					QuestNode_GiveRewards.tmpPrevQuestParts.AddRange(QuestGen.quest.PartsListForReading);
					this.nodeIfChosenPawnSignalUsed.Run();
					List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
					for (int k = 0; k < partsListForReading.Count; k++)
					{
						if (!QuestNode_GiveRewards.tmpPrevQuestParts.Contains(partsListForReading[k]))
						{
							for (int l = 0; l < questPart_Choice.choices.Count; l++)
							{
								bool flag3 = false;
								for (int m = 0; m < questPart_Choice.choices[l].rewards.Count; m++)
								{
									if (questPart_Choice.choices[l].rewards[m].MakesUseOfChosenPawnSignal)
									{
										flag3 = true;
										break;
									}
								}
								if (flag3)
								{
									questPart_Choice.choices[l].questParts.Add(partsListForReading[k]);
								}
							}
						}
					}
					QuestNode_GiveRewards.tmpPrevQuestParts.Clear();
				}
			}
			finally
			{
				slate.Restore(restoreInfo);
			}
		}

		
		private List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants)
		{
			List<string> list;
			List<string> list2;
			if (addDescriptionRules)
			{
				list = new List<string>();
				list2 = new List<string>();
			}
			else
			{
				list = null;
				list2 = null;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.generatedRewards.Count; i++)
			{
				for (int j = 0; j < this.generatedRewards[i].Count; j++)
				{
					if (this.generatedRewards[i][j] is Reward_Pawn)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					break;
				}
			}
			if (flag2)
			{
				parmsResolved.thingRewardItemsOnly = true;
			}
			List<Reward> list3 = null;
			if (variants >= 2 && !this.generatedRewards.Any<List<Reward>>() && parmsResolved.allowGoodwill && !parmsResolved.thingRewardRequired)
			{
				list3 = this.TryGenerateRewards_SocialOnly(parmsResolved, variants >= 3);
				if (list3.NullOrEmpty<Reward>() && variants >= 3)
				{
					list3 = this.TryGenerateRewards_ThingsOnly(parmsResolved);
				}
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = this.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else if (variants >= 3 && this.generatedRewards.Count == 1 && parmsResolved.allowRoyalFavor && !parmsResolved.thingRewardRequired)
			{
				list3 = this.TryGenerateRewards_RoyalFavorOnly(parmsResolved);
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = this.TryGenerateRewards_ThingsOnly(parmsResolved);
				}
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = this.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else if (variants >= 2 && this.generatedRewards.Any<List<Reward>>() && !parmsResolved.thingRewardDisallowed)
			{
				list3 = this.TryGenerateRewards_ThingsOnly(parmsResolved);
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = this.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else
			{
				list3 = this.TryGenerateNonRepeatingRewards(parmsResolved);
			}
			if (list3.NullOrEmpty<Reward>())
			{
				return null;
			}
			Reward_Items reward_Items = (Reward_Items)list3.Find((Reward x) => x is Reward_Items);
			if (reward_Items == null)
			{
				List<Type> b = (from x in list3
				select x.GetType()).ToList<Type>();
				for (int k = 0; k < this.generatedRewards.Count; k++)
				{
					if ((from x in this.generatedRewards[k]
					select x.GetType()).ToList<Type>().ListsEqualIgnoreOrder(b))
					{
						return null;
					}
				}
			}
			else if (list3.Count == 1)
			{
				List<ThingDef> b2 = (from x in reward_Items.ItemsListForReading
				select x.def).ToList<ThingDef>();
				for (int l = 0; l < this.generatedRewards.Count; l++)
				{
					Reward_Items reward_Items2 = (Reward_Items)this.generatedRewards[l].Find((Reward x) => x is Reward_Items);
					if (reward_Items2 != null)
					{
						if ((from x in reward_Items2.ItemsListForReading
						select x.def).ToList<ThingDef>().ListsEqualIgnoreOrder(b2))
						{
							return null;
						}
					}
				}
			}
			list3.SortBy((Reward x) => x is Reward_Items);
			choice.rewards.AddRange(list3);
			for (int m = 0; m < list3.Count; m++)
			{
				if (addDescriptionRules)
				{
					list.Add(list3[m].GetDescription(parmsResolved));
					if (!(list3[m] is Reward_Items))
					{
						list2.Add(list3[m].GetDescription(parmsResolved));
					}
					else if (m == list3.Count - 1)
					{
						flag = true;
					}
				}
				foreach (QuestPart questPart in list3[m].GenerateQuestParts(m, parmsResolved, this.customLetterLabel.GetValue(slate), this.customLetterText.GetValue(slate), this.customLetterLabelRules.GetValue(slate), this.customLetterTextRules.GetValue(slate)))
				{
					QuestGen.quest.AddPart(questPart);
					choice.questParts.Add(questPart);
				}
				if (!this.parms.GetValue(slate).chosenPawnSignal.NullOrEmpty() && list3[m].MakesUseOfChosenPawnSignal)
				{
					chosenPawnSignalUsed = true;
				}
			}
			if (addDescriptionRules)
			{
				string text = list.AsEnumerable<string>().ToList<string>().ToClauseSequence().Resolve().UncapitalizeFirst();
				if (flag)
				{
					text = text.TrimEnd(new char[]
					{
						'.'
					});
				}
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allRewardsDescriptions", text.UncapitalizeFirst()),
					new Rule_String("allRewardsDescriptionsExceptItems", list2.Any<string>() ? list2.AsEnumerable<string>().ToList<string>().ToClauseSequence().Resolve().UncapitalizeFirst() : "")
				});
			}
			return list3;
		}

		
		private List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			parms.thingRewardDisallowed = true;
			if (disallowRoyalFavor)
			{
				parms.allowRoyalFavor = false;
			}
			return this.TryGenerateNonRepeatingRewards(parms);
		}

		
		private List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			parms.allowGoodwill = false;
			parms.thingRewardDisallowed = true;
			return this.TryGenerateNonRepeatingRewards(parms);
		}

		
		private List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			if (parms.thingRewardDisallowed)
			{
				return null;
			}
			parms.allowGoodwill = false;
			parms.allowRoyalFavor = false;
			return this.TryGenerateNonRepeatingRewards(parms);
		}

		
		private List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			List<Reward> list = null;
			int i = 0;
			while (i < 10)
			{
				list = RewardsGenerator.Generate(parms);
				if (list.Any((Reward x) => x is Reward_Pawn))
				{
					return list;
				}
				Reward_Items reward_Items = (Reward_Items)list.FirstOrDefault((Reward x) => x is Reward_Items);
				if (reward_Items != null)
				{
					bool flag = false;
					for (int j = 0; j < this.generatedRewards.Count; j++)
					{
						Reward_Items otherGeneratedItems = null;
						for (int l = 0; l < this.generatedRewards[j].Count; l++)
						{
							otherGeneratedItems = (this.generatedRewards[j][l] as Reward_Items);
							if (otherGeneratedItems != null)
							{
								break;
							}
						}
						if (otherGeneratedItems != null)
						{
							int k2;
							int k;
							for (k = 0; k < otherGeneratedItems.items.Count; k = k2 + 1)
							{
								if (reward_Items.items.Any((Thing x) => x.GetInnerIfMinified().def == otherGeneratedItems.items[k].GetInnerIfMinified().def))
								{
									flag = true;
									break;
								}
								k2 = k;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						i++;
						continue;
					}
				}
				return list;
			}
			return list;
		}

		
		private int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			for (int i = 0; i < choice.rewards.Count; i++)
			{
				if (choice.rewards[i] is Reward_RoyalFavor)
				{
					return 1;
				}
			}
			for (int j = 0; j < choice.rewards.Count; j++)
			{
				if (choice.rewards[j] is Reward_Goodwill)
				{
					return -1;
				}
			}
			return 0;
		}

		
		[NoTranslate]
		public SlateRef<string> inSignal;

		
		public SlateRef<RewardsGeneratorParams> parms;

		
		public SlateRef<string> customLetterLabel;

		
		public SlateRef<string> customLetterText;

		
		public SlateRef<RulePack> customLetterLabelRules;

		
		public SlateRef<RulePack> customLetterTextRules;

		
		public SlateRef<bool?> useDifficultyFactor;

		
		public QuestNode nodeIfChosenPawnSignalUsed;

		
		public SlateRef<int?> variants;

		
		public SlateRef<bool> addCampLootReward;

		
		private List<List<Reward>> generatedRewards = new List<List<Reward>>();

		
		private const float MinRewardValue = 250f;

		
		private const int DefaultVariants = 3;

		
		private static List<QuestPart> tmpPrevQuestParts = new List<QuestPart>();
	}
}
