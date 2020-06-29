using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class TraderKindDef : Def
	{
		
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

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		
		public bool orbital;

		
		public bool requestable = true;

		
		public bool hideThingsNotWillingToTrade;

		
		public float commonality = 1f;

		
		public string category;

		
		public TradeCurrency tradeCurrency;

		
		public SimpleCurve commonalityMultFromPopulationIntent;

		
		public FactionDef faction;

		
		public RoyalTitlePermitDef permitRequiredForTrading;
	}
}
