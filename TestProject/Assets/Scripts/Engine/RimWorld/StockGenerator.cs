using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAC RID: 3500
	public abstract class StockGenerator
	{
		// Token: 0x06005508 RID: 21768 RVA: 0x001C4C84 File Offset: 0x001C2E84
		public virtual void ResolveReferences(TraderKindDef trader)
		{
			this.trader = trader;
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x001C4C8D File Offset: 0x001C2E8D
		public virtual IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			yield break;
		}

		// Token: 0x0600550A RID: 21770
		public abstract IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null);

		// Token: 0x0600550B RID: 21771
		public abstract bool HandlesThingDef(ThingDef thingDef);

		// Token: 0x0600550C RID: 21772 RVA: 0x001C4C96 File Offset: 0x001C2E96
		public bool TryGetPriceType(ThingDef thingDef, TradeAction action, out PriceType priceType)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				priceType = PriceType.Undefined;
				return false;
			}
			priceType = this.price;
			return true;
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x001C4CB0 File Offset: 0x001C2EB0
		protected int RandomCountOf(ThingDef def)
		{
			IntRange intRange = this.countRange;
			if (this.customCountRanges != null)
			{
				for (int i = 0; i < this.customCountRanges.Count; i++)
				{
					if (this.customCountRanges[i].thingDef == def)
					{
						intRange = this.customCountRanges[i].countRange;
						break;
					}
				}
			}
			if (intRange.max <= 0 && this.totalPriceRange.max <= 0f)
			{
				return 0;
			}
			if (intRange.max > 0 && this.totalPriceRange.max <= 0f)
			{
				return intRange.RandomInRange;
			}
			if (intRange.max <= 0 && this.totalPriceRange.max > 0f)
			{
				return Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
			int num = 0;
			int randomInRange;
			do
			{
				randomInRange = intRange.RandomInRange;
				num++;
			}
			while (num <= 100 && !this.totalPriceRange.Includes((float)randomInRange * def.BaseMarketValue));
			return randomInRange;
		}

		// Token: 0x04002E91 RID: 11921
		[Unsaved(false)]
		public TraderKindDef trader;

		// Token: 0x04002E92 RID: 11922
		public IntRange countRange = IntRange.zero;

		// Token: 0x04002E93 RID: 11923
		public List<ThingDefCountRangeClass> customCountRanges;

		// Token: 0x04002E94 RID: 11924
		public FloatRange totalPriceRange = FloatRange.Zero;

		// Token: 0x04002E95 RID: 11925
		public TechLevel maxTechLevelGenerate = TechLevel.Archotech;

		// Token: 0x04002E96 RID: 11926
		public TechLevel maxTechLevelBuy = TechLevel.Archotech;

		// Token: 0x04002E97 RID: 11927
		public PriceType price = PriceType.Normal;
	}
}
