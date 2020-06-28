using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD9 RID: 4057
	public class Reward_RoyalFavor : Reward
	{
		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x06006174 RID: 24948 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x06006175 RID: 24949 RVA: 0x0021D3A3 File Offset: 0x0021B5A3
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

		// Token: 0x06006176 RID: 24950 RVA: 0x0021D3B4 File Offset: 0x0021B5B4
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
			this.amount = Mathf.Clamp(this.amount, 1, 12);
			valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted((float)this.amount);
			this.faction = parms.giverFaction;
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x0021D40A File Offset: 0x0021B60A
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

		// Token: 0x06006178 RID: 24952 RVA: 0x0021D424 File Offset: 0x0021B624
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				return "Reward_RoyalFavor_ChoosePawn".Translate(this.faction, this.amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
			}
			return "Reward_RoyalFavor".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x0021D4A4 File Offset: 0x0021B6A4
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

		// Token: 0x0600617A RID: 24954 RVA: 0x0021D4FE File Offset: 0x0021B6FE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04003B47 RID: 15175
		public int amount;

		// Token: 0x04003B48 RID: 15176
		public Faction faction;
	}
}
