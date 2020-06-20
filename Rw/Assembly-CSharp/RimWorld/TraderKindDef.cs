using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000916 RID: 2326
	public class TraderKindDef : Def
	{
		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003744 RID: 14148 RVA: 0x00129554 File Offset: 0x00127754
		public float CalculatedCommonality
		{
			get
			{
				float num = this.commonality;
				if (this.commonalityMultFromPopulationIntent != null)
				{
					num *= this.commonalityMultFromPopulationIntent.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
				}
				return num;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x00129584 File Offset: 0x00127784
		public RoyalTitleDef TitleRequiredToTrade
		{
			get
			{
				if (this.permitRequiredForTrading != null)
				{
					RoyalTitleDef royalTitleDef = this.faction.RoyalTitlesAwardableInSeniorityOrderForReading.FirstOrDefault((RoyalTitleDef x) => x.permits != null && x.permits.Contains(this.permitRequiredForTrading));
					if (royalTitleDef != null)
					{
						return royalTitleDef;
					}
				}
				return null;
			}
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x001295BC File Offset: 0x001277BC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x00129614 File Offset: 0x00127814
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				foreach (string text2 in stockGenerator.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			List<StockGenerator>.Enumerator enumerator2 = default(List<StockGenerator>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x00129624 File Offset: 0x00127824
		public bool WillTrade(ThingDef td)
		{
			for (int i = 0; i < this.stockGenerators.Count; i++)
			{
				if (this.stockGenerators[i].HandlesThingDef(td))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x00129660 File Offset: 0x00127860
		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			if (thingDef == ThingDefOf.Silver)
			{
				return PriceType.Undefined;
			}
			if (action == TradeAction.PlayerBuys)
			{
				for (int i = 0; i < this.stockGenerators.Count; i++)
				{
					PriceType result;
					if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result))
					{
						return result;
					}
				}
			}
			return PriceType.Normal;
		}

		// Token: 0x04002077 RID: 8311
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x04002078 RID: 8312
		public bool orbital;

		// Token: 0x04002079 RID: 8313
		public bool requestable = true;

		// Token: 0x0400207A RID: 8314
		public bool hideThingsNotWillingToTrade;

		// Token: 0x0400207B RID: 8315
		public float commonality = 1f;

		// Token: 0x0400207C RID: 8316
		public string category;

		// Token: 0x0400207D RID: 8317
		public TradeCurrency tradeCurrency;

		// Token: 0x0400207E RID: 8318
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x0400207F RID: 8319
		public FactionDef faction;

		// Token: 0x04002080 RID: 8320
		public RoyalTitlePermitDef permitRequiredForTrading;
	}
}
