using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000911 RID: 2321
	public struct ThingSetMakerParams
	{
		// Token: 0x0400203B RID: 8251
		public TechLevel? techLevel;

		// Token: 0x0400203C RID: 8252
		public IntRange? countRange;

		// Token: 0x0400203D RID: 8253
		public ThingFilter filter;

		// Token: 0x0400203E RID: 8254
		public Predicate<ThingDef> validator;

		// Token: 0x0400203F RID: 8255
		public QualityGenerator? qualityGenerator;

		// Token: 0x04002040 RID: 8256
		public float? maxTotalMass;

		// Token: 0x04002041 RID: 8257
		public float? maxThingMarketValue;

		// Token: 0x04002042 RID: 8258
		public bool? allowNonStackableDuplicates;

		// Token: 0x04002043 RID: 8259
		public float? minSingleItemMarketValuePct;

		// Token: 0x04002044 RID: 8260
		public FloatRange? totalMarketValueRange;

		// Token: 0x04002045 RID: 8261
		public FloatRange? totalNutritionRange;

		// Token: 0x04002046 RID: 8262
		public PodContentsType? podContentsType;

		// Token: 0x04002047 RID: 8263
		public Faction makingFaction;

		// Token: 0x04002048 RID: 8264
		public TraderKindDef traderDef;

		// Token: 0x04002049 RID: 8265
		public int? tile;

		// Token: 0x0400204A RID: 8266
		public Dictionary<string, object> custom;
	}
}
