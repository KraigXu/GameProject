using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Reward_Goodwill : Reward
	{
		
		
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Goodwill".Translate() + " " + this.amount.ToStringWithSign(), delegate(Rect r)
				{
					GUI.color = this.faction.Color;
					GUI.DrawTexture(r, this.faction.def.FactionIcon);
					GUI.color = Color.white;
				}, () => "GoodwillTip".Translate(this.faction, this.amount, -75, 75, this.faction.PlayerGoodwill, this.faction.PlayerRelationKind.GetLabel()), delegate
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToGoodwillCurve.Evaluate(rewardValue));
			this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
			this.amount = Mathf.Max(this.amount, 1);
			valueActuallyUsed = RewardsGenerator.RewardValueToGoodwillCurve.EvaluateInverted((float)this.amount);
			if (parms.giverFaction.HostileTo(Faction.OfPlayer))
			{
				this.amount += Mathf.Clamp(-parms.giverFaction.PlayerGoodwill / 2, 0, this.amount);
				this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
				this.amount = Mathf.Max(this.amount, 1);
			}
			this.faction = parms.giverFaction;
		}

		
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			yield return new QuestPart_FactionGoodwillChange
            {
				change = this.amount,
				faction = this.faction,
				inSignal = QuestGen.QuestGen.slate.Get<string>("inSignal", null, false)
			};
			yield break;
		}

		
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Goodwill".Translate(this.faction, this.amount).Resolve();
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
