using System;
using System.Collections.Generic;
using RimWorld.QuestGenNew;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Reward_RoyalFavor : Reward
	{
		
		
		public override bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return true;
			}
		}

		
		
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement(this.faction.def.royalFavorLabel.CapitalizeFirst() + " " + this.amount.ToStringWithSign(), this.faction.def.RoyalFavorIcon, () => "RoyalFavorTip".Translate(Faction.OfPlayer.def.pawnsPlural, this.amount, this.faction.def.royalFavorLabel, this.faction) + "\n\n" + "ClickForMoreInfo".Translate(), delegate
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
			this.amount = Mathf.Clamp(this.amount, 1, 12);
			valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted((float)this.amount);
			this.faction = parms.giverFaction;
		}

		
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.faction = this.faction;
			questPart_GiveRoyalFavor.amount = this.amount;
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				questPart_GiveRoyalFavor.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(parms.chosenPawnSignal);
				questPart_GiveRoyalFavor.signalListenMode = QuestPart.SignalListenMode.Always;
			}
			else
			{
				questPart_GiveRoyalFavor.inSignal = slate.Get<string>("inSignal", null, false);
				questPart_GiveRoyalFavor.giveToAccepter = true;
			}
			yield return questPart_GiveRoyalFavor;
			slate.Set<int>("royalFavorReward_amount", this.amount, false);
			yield break;
		}

		
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				return "Reward_RoyalFavor_ChoosePawn".Translate(this.faction, this.amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
			}
			return "Reward_RoyalFavor".Translate(this.faction, this.amount).Resolve();
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" (faction=",
				this.faction.ToStringSafe<Faction>(),
				", amount=",
				this.amount,
				")"
			});
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		
		public int amount;

		
		public Faction faction;
	}
}
