﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		
		// (get) Token: 0x0600643C RID: 25660 RVA: 0x0022B7D5 File Offset: 0x002299D5
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		
		// (get) Token: 0x0600643D RID: 25661 RVA: 0x0022B7DD File Offset: 0x002299DD
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		
		// (get) Token: 0x0600643E RID: 25662 RVA: 0x0022B7E5 File Offset: 0x002299E5
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		
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

		
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.commonalityMultiplier);
		}

		
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		
		public ThingDef thing;

		
		public ThingDef stuff;

		
		public float commonalityMultiplier;

		
		private float cachedPrice;

		
		private float cachedInsulationCold;

		
		private float cachedInsulationHeat;
	}
}
