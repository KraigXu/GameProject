using System;

namespace Verse
{
	// Token: 0x0200041D RID: 1053
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000C15BB File Offset: 0x000BF7BB
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x000C15C3 File Offset: 0x000BF7C3
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x000C15CB File Offset: 0x000BF7CB
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x000C15D8 File Offset: 0x000BF7D8
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x000C15E5 File Offset: 0x000BF7E5
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x000C15F2 File Offset: 0x000BF7F2
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000C15FF File Offset: 0x000BF7FF
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000C160F File Offset: 0x000BF80F
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x000C1620 File Offset: 0x000BF820
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000C1657 File Offset: 0x000BF857
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000C1665 File Offset: 0x000BF865
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x000C1674 File Offset: 0x000BF874
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000C168C File Offset: 0x000BF88C
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000C169A File Offset: 0x000BF89A
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x000C16BD File Offset: 0x000BF8BD
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x000C16C9 File Offset: 0x000BF8C9
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000C16E8 File Offset: 0x000BF8E8
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

		// Token: 0x06001F96 RID: 8086 RVA: 0x000C1743 File Offset: 0x000BF943
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000C1756 File Offset: 0x000BF956
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000C1772 File Offset: 0x000BF972
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x04001320 RID: 4896
		private ThingDef thingDef;

		// Token: 0x04001321 RID: 4897
		private IntRange countRange;
	}
}
