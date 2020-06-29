using System;

namespace Verse
{
	
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000C15BB File Offset: 0x000BF7BB
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x000C15C3 File Offset: 0x000BF7C3
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x000C15CB File Offset: 0x000BF7CB
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x000C15D8 File Offset: 0x000BF7D8
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x000C15E5 File Offset: 0x000BF7E5
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x000C15F2 File Offset: 0x000BF7F2
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.countRange,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		
		private ThingDef thingDef;

		
		private IntRange countRange;
	}
}
