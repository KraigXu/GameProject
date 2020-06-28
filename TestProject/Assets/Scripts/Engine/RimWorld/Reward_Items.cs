using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FD6 RID: 4054
	public class Reward_Items : Reward
	{
		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x0600615A RID: 24922 RVA: 0x0021CCA0 File Offset: 0x0021AEA0
		public List<Thing> ItemsListForReading
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x0021CCA8 File Offset: 0x0021AEA8
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					foreach (GenUI.AnonymousStackElement anonymousStackElement in QuestPartUtility.GetRewardStackElementsForThings(this.itemDefs))
					{
						yield return anonymousStackElement;
					}
					IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				}
				else
				{
					foreach (GenUI.AnonymousStackElement anonymousStackElement2 in QuestPartUtility.GetRewardStackElementsForThings(this.items, false))
					{
						yield return anonymousStackElement2;
					}
					IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x0600615C RID: 24924 RVA: 0x0021CCB8 File Offset: 0x0021AEB8
		public override float TotalMarketValue
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					return this.lastTotalMarketValue;
				}
				float num = 0f;
				for (int i = 0; i < this.items.Count; i++)
				{
					Thing innerIfMinified = this.items[i].GetInnerIfMinified();
					num += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0600615D RID: 24925 RVA: 0x0021CD1F File Offset: 0x0021AF1F
		public override void Notify_Used()
		{
			this.RememberItems();
			base.Notify_Used();
		}

		// Token: 0x0600615E RID: 24926 RVA: 0x0021CD2D File Offset: 0x0021AF2D
		public override void Notify_PreCleanup()
		{
			this.RememberItems();
			base.Notify_PreCleanup();
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x0021CD3C File Offset: 0x0021AF3C
		private void RememberItems()
		{
			if (this.usedOrCleanedUp)
			{
				return;
			}
			this.itemDefs.Clear();
			this.lastTotalMarketValue = 0f;
			for (int i = 0; i < this.items.Count; i++)
			{
				Thing innerIfMinified = this.items[i].GetInnerIfMinified();
				if (!innerIfMinified.Destroyed)
				{
					QualityCategory quality;
					if (!innerIfMinified.TryGetQuality(out quality))
					{
						quality = QualityCategory.Normal;
					}
					this.itemDefs.Add(new Reward_Items.RememberedItem(new ThingStuffPairWithQuality(innerIfMinified.def, innerIfMinified.Stuff, quality), this.items[i].stackCount, this.items[i].LabelNoCount));
					this.lastTotalMarketValue += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
			}
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x0021CE14 File Offset: 0x0021B014
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.items.Clear();
			bool flag = true;
			float x2 = (Find.TickManager.TicksGame - Find.History.lastPsylinkAvailable).TicksToDays();
			if (Rand.Chance(QuestTuning.DaysSincePsylinkAvailableToGuaranteedNeuroformerChance.Evaluate(x2)) && ModsConfig.RoyaltyActive && (parms.disallowedThingDefs == null || !parms.disallowedThingDefs.Contains(ThingDefOf.PsychicAmplifier)) && rewardValue >= 600f && parms.giverFaction != Faction.Empire)
			{
				this.items.Add(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null));
				rewardValue -= this.items[0].MarketValue;
				if (rewardValue < 100f)
				{
					flag = false;
				}
			}
			if (flag)
			{
				FloatRange value = rewardValue * new FloatRange(0.7f, 1.3f);
				ThingSetMakerParams parms2 = default(ThingSetMakerParams);
				parms2.totalMarketValueRange = new FloatRange?(value);
				parms2.makingFaction = parms.giverFaction;
				if (!parms.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					parms2.validator = ((ThingDef x) => !parms.disallowedThingDefs.Contains(x));
				}
				this.items.AddRange(ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms2));
			}
			valueActuallyUsed = this.TotalMarketValue;
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x0021CF6B File Offset: 0x0021B16B
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.items.Count; i++)
			{
				Pawn pawn = this.items[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					inSignal = slate.Get<string>("inSignal", null, false),
					Things = this.items
				};
			}
			else
			{
				QuestPart_DropPods dropPods = new QuestPart_DropPods();
				dropPods.inSignal = slate.Get<string>("inSignal", null, false);
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						dropPods.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						dropPods.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				dropPods.mapParent = slate.Get<Map>("map", null, false).Parent;
				dropPods.useTradeDropSpot = true;
				dropPods.Things = this.items;
				yield return dropPods;
			}
			slate.Set<List<Thing>>("itemsReward_items", this.items, false);
			slate.Set<float>("itemsReward_totalMarketValue", this.TotalMarketValue, false);
			yield break;
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x0021CFA4 File Offset: 0x0021B1A4
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
			}
			return "Reward_Items".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
		}

		// Token: 0x06006163 RID: 24931 RVA: 0x0021D024 File Offset: 0x0021B224
		public override string ToString()
		{
			string text = base.GetType().Name;
			text = text + "(value " + this.TotalMarketValue.ToStringMoney(null) + ")";
			foreach (Thing thing in this.items)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n  -",
					thing.LabelCap,
					" ",
					(thing.MarketValue * (float)thing.stackCount).ToStringMoney(null)
				});
			}
			return text;
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x0021D0DC File Offset: 0x0021B2DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Reward_Items.RememberedItem>(ref this.itemDefs, "itemDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.lastTotalMarketValue, "lastTotalMarketValue", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04003B3E RID: 15166
		public List<Thing> items = new List<Thing>();

		// Token: 0x04003B3F RID: 15167
		private List<Reward_Items.RememberedItem> itemDefs = new List<Reward_Items.RememberedItem>();

		// Token: 0x04003B40 RID: 15168
		private float lastTotalMarketValue;

		// Token: 0x04003B41 RID: 15169
		private const string RootSymbol = "root";

		// Token: 0x02001E74 RID: 7796
		public struct RememberedItem : IExposable
		{
			// Token: 0x0600A941 RID: 43329 RVA: 0x00319897 File Offset: 0x00317A97
			public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
			{
				this.thing = thing;
				this.stackCount = stackCount;
				this.label = label;
			}

			// Token: 0x0600A942 RID: 43330 RVA: 0x003198AE File Offset: 0x00317AAE
			public void ExposeData()
			{
				Scribe_Deep.Look<ThingStuffPairWithQuality>(ref this.thing, "thing", Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Values.Look<string>(ref this.label, "label", null, false);
			}

			// Token: 0x04007287 RID: 29319
			public ThingStuffPairWithQuality thing;

			// Token: 0x04007288 RID: 29320
			public int stackCount;

			// Token: 0x04007289 RID: 29321
			public string label;
		}
	}
}
