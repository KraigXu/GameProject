using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Reward_Items : Reward
	{
		
		// (get) Token: 0x0600615A RID: 24922 RVA: 0x0021CCA0 File Offset: 0x0021AEA0
		public List<Thing> ItemsListForReading
		{
			get
			{
				return this.items;
			}
		}

		
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

		
		public override void Notify_Used()
		{
			this.RememberItems();
			base.Notify_Used();
		}

		
		public override void Notify_PreCleanup()
		{
			this.RememberItems();
			base.Notify_PreCleanup();
		}

		
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

		
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
			}
			return "Reward_Items".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
		}

		
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

		
		public List<Thing> items = new List<Thing>();

		
		private List<Reward_Items.RememberedItem> itemDefs = new List<Reward_Items.RememberedItem>();

		
		private float lastTotalMarketValue;

		
		private const string RootSymbol = "root";

		
		public struct RememberedItem : IExposable
		{
			
			public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
			{
				this.thing = thing;
				this.stackCount = stackCount;
				this.label = label;
			}

			
			public void ExposeData()
			{
				Scribe_Deep.Look<ThingStuffPairWithQuality>(ref this.thing, "thing", Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Values.Look<string>(ref this.label, "label", null, false);
			}

			
			public ThingStuffPairWithQuality thing;

			
			public int stackCount;

			
			public string label;
		}
	}
}
