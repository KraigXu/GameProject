﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCC RID: 3276
	public class ThingSetMaker_Sum : ThingSetMaker
	{
		// Token: 0x06004F61 RID: 20321 RVA: 0x001AB8EC File Offset: 0x001A9AEC
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f && this.options[i].thingSetMaker.CanGenerate(parms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001AB944 File Offset: 0x001A9B44
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			this.optionsInResolveOrder.Clear();
			this.optionsInResolveOrder.AddRange(this.options);
			if (!this.resolveInOrder)
			{
				this.optionsInResolveOrder.Shuffle<ThingSetMaker_Sum.Option>();
			}
			int i = 0;
			while (i < this.optionsInResolveOrder.Count)
			{
				ThingSetMakerParams parms2 = parms;
				if (parms2.countRange == null)
				{
					goto IL_B0;
				}
				parms2.countRange = new IntRange?(new IntRange(Mathf.Max(parms2.countRange.Value.min - num, 0), parms2.countRange.Value.max - num));
				if (parms2.countRange.Value.max > 0)
				{
					goto IL_B0;
				}
				IL_3B4:
				i++;
				continue;
				IL_B0:
				if (parms2.totalMarketValueRange != null)
				{
					if (parms2.totalMarketValueRange.Value.max < this.optionsInResolveOrder[i].minTotalMarketValue)
					{
						goto IL_3B4;
					}
					parms2.totalMarketValueRange = new FloatRange?(new FloatRange(Mathf.Max(parms2.totalMarketValueRange.Value.min - num2, 0f), parms2.totalMarketValueRange.Value.max - num2));
					if (this.optionsInResolveOrder[i].maxMarketValue != null)
					{
						parms2.totalMarketValueRange = new FloatRange?(new FloatRange(Mathf.Min(parms2.totalMarketValueRange.Value.min, this.optionsInResolveOrder[i].maxMarketValue.Value * 0.8f), Mathf.Min(new float[]
						{
							Mathf.Min(parms2.totalMarketValueRange.Value.max, this.optionsInResolveOrder[i].maxMarketValue.Value)
						})));
					}
					if (parms2.totalMarketValueRange.Value.max <= 0f || parms2.totalMarketValueRange.Value.max < this.optionsInResolveOrder[i].minMarketValue)
					{
						goto IL_3B4;
					}
				}
				if (parms2.totalNutritionRange != null)
				{
					parms2.totalNutritionRange = new FloatRange?(new FloatRange(Mathf.Max(parms2.totalNutritionRange.Value.min - num3, 0f), parms2.totalNutritionRange.Value.max - num3));
				}
				if (parms2.maxTotalMass != null)
				{
					parms2.maxTotalMass -= num4;
				}
				if (Rand.Chance(this.optionsInResolveOrder[i].chance) && this.optionsInResolveOrder[i].thingSetMaker.CanGenerate(parms2))
				{
					List<Thing> list = this.optionsInResolveOrder[i].thingSetMaker.Generate(parms2);
					num += list.Count;
					for (int j = 0; j < list.Count; j++)
					{
						num2 += list[j].MarketValue * (float)list[j].stackCount;
						if (list[j].def.IsIngestible)
						{
							num3 += list[j].GetStatValue(StatDefOf.Nutrition, true) * (float)list[j].stackCount;
						}
						if (!(list[j] is Pawn))
						{
							num4 += list[j].GetStatValue(StatDefOf.Mass, true) * (float)list[j].stackCount;
						}
					}
					outThings.AddRange(list);
					goto IL_3B4;
				}
				goto IL_3B4;
			}
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x001ABD20 File Offset: 0x001A9F20
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x001ABD5F File Offset: 0x001A9F5F
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			int num;
			for (int i = 0; i < this.options.Count; i = num + 1)
			{
				if (this.options[i].chance > 0f)
				{
					foreach (ThingDef thingDef in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return thingDef;
					}
					IEnumerator<ThingDef> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04002C8A RID: 11402
		public List<ThingSetMaker_Sum.Option> options;

		// Token: 0x04002C8B RID: 11403
		public bool resolveInOrder;

		// Token: 0x04002C8C RID: 11404
		private List<ThingSetMaker_Sum.Option> optionsInResolveOrder = new List<ThingSetMaker_Sum.Option>();

		// Token: 0x02001C1A RID: 7194
		public class Option
		{
			// Token: 0x04006A85 RID: 27269
			public ThingSetMaker thingSetMaker;

			// Token: 0x04006A86 RID: 27270
			public float chance = 1f;

			// Token: 0x04006A87 RID: 27271
			public float? maxMarketValue;

			// Token: 0x04006A88 RID: 27272
			public float minMarketValue;

			// Token: 0x04006A89 RID: 27273
			public float minTotalMarketValue;

			// Token: 0x04006A8A RID: 27274
			public const float MaxMarketValueLeewayFactor = 0.8f;
		}
	}
}
