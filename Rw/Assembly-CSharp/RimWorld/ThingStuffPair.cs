using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001080 RID: 4224
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x0600643C RID: 25660 RVA: 0x0022B7D5 File Offset: 0x002299D5
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x0600643D RID: 25661 RVA: 0x0022B7DD File Offset: 0x002299DD
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x0600643E RID: 25662 RVA: 0x0022B7E5 File Offset: 0x002299E5
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x0600643F RID: 25663 RVA: 0x0022B7F0 File Offset: 0x002299F0
		public float Commonality
		{
			get
			{
				float num = this.commonalityMultiplier;
				num *= this.thing.generateCommonality;
				if (this.stuff != null)
				{
					num *= this.stuff.stuffProps.commonality;
				}
				if (PawnWeaponGenerator.IsDerpWeapon(this.thing, this.stuff) || PawnApparelGenerator.IsDerpApparel(this.thing, this.stuff))
				{
					num *= 0.01f;
				}
				return num;
			}
		}

		// Token: 0x06006440 RID: 25664 RVA: 0x0022B85C File Offset: 0x00229A5C
		public ThingStuffPair(ThingDef thing, ThingDef stuff, float commonalityMultiplier = 1f)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.commonalityMultiplier = commonalityMultiplier;
			if (stuff != null && !thing.MadeFromStuff)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with stuff ",
					stuff,
					" but ",
					thing,
					" is not made from stuff."
				}), false);
				stuff = null;
			}
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff);
			this.cachedInsulationHeat = thing.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff);
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x0022B8F4 File Offset: 0x00229AF4
		public static List<ThingStuffPair> AllWith(Predicate<ThingDef> thingValidator)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingValidator(thingDef))
				{
					if (!thingDef.MadeFromStuff)
					{
						list.Add(new ThingStuffPair(thingDef, null, 1f));
					}
					else
					{
						IEnumerable<ThingDef> enumerable = from st in DefDatabase<ThingDef>.AllDefs
						where st.IsStuff && st.stuffProps.CanMake(thingDef)
						select st;
						int num = enumerable.Count<ThingDef>();
						float num2 = enumerable.Average((ThingDef st) => st.stuffProps.commonality);
						float num3 = 1f / (float)num / num2;
						foreach (ThingDef thingDef2 in enumerable)
						{
							list.Add(new ThingStuffPair(thingDef, thingDef2, num3));
						}
					}
				}
			}
			return (from p in list
			orderby p.Price descending
			select p).ToList<ThingStuffPair>();
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x0022BA3C File Offset: 0x00229C3C
		public override string ToString()
		{
			if (this.thing == null)
			{
				return "(null)";
			}
			string text;
			if (this.stuff == null)
			{
				text = this.thing.label;
			}
			else
			{
				text = this.thing.label + " " + this.stuff.LabelAsStuff;
			}
			return string.Concat(new string[]
			{
				text,
				" $",
				this.Price.ToString("F0"),
				" c=",
				this.Commonality.ToString("F4")
			});
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x0022BAD9 File Offset: 0x00229CD9
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x0022BB07 File Offset: 0x00229D07
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x06006445 RID: 25669 RVA: 0x0022BB13 File Offset: 0x00229D13
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x0022BB2B File Offset: 0x00229D2B
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x0022BB39 File Offset: 0x00229D39
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.commonalityMultiplier);
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x0022BB5D File Offset: 0x00229D5D
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x04003CF6 RID: 15606
		public ThingDef thing;

		// Token: 0x04003CF7 RID: 15607
		public ThingDef stuff;

		// Token: 0x04003CF8 RID: 15608
		public float commonalityMultiplier;

		// Token: 0x04003CF9 RID: 15609
		private float cachedPrice;

		// Token: 0x04003CFA RID: 15610
		private float cachedInsulationCold;

		// Token: 0x04003CFB RID: 15611
		private float cachedInsulationHeat;
	}
}
