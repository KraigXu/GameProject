using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD5 RID: 4053
	public class Reward_Goodwill : Reward
	{
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06006150 RID: 24912 RVA: 0x0021CA33 File Offset: 0x0021AC33
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

		// Token: 0x06006151 RID: 24913 RVA: 0x0021CA44 File Offset: 0x0021AC44
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

		// Token: 0x06006152 RID: 24914 RVA: 0x0021CB21 File Offset: 0x0021AD21
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			yield return new QuestPart_FactionGoodwillChange
			{
				change = this.amount,
				faction = this.faction,
				inSignal = QuestGen.slate.Get<string>("inSignal", null, false)
			};
			yield break;
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x0021CB34 File Offset: 0x0021AD34
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Goodwill".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x06006154 RID: 24916 RVA: 0x0021CB6C File Offset: 0x0021AD6C
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

		// Token: 0x06006155 RID: 24917 RVA: 0x0021CBC6 File Offset: 0x0021ADC6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04003B3C RID: 15164
		public int amount;

		// Token: 0x04003B3D RID: 15165
		public Faction faction;
	}
}
