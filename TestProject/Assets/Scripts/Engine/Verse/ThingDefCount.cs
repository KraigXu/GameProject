using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041B RID: 1051
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x000C1268 File Offset: 0x000BF468
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x000C1270 File Offset: 0x000BF470
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x000C1278 File Offset: 0x000BF478
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x000C128C File Offset: 0x000BF48C
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000C12A0 File Offset: 0x000BF4A0
		public ThingDefCount(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCount count to ",
					count,
					". thingDef=",
					thingDef
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000C12F0 File Offset: 0x000BF4F0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x000C1314 File Offset: 0x000BF514
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000C1322 File Offset: 0x000BF522
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x000C133A File Offset: 0x000BF53A
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000C1348 File Offset: 0x000BF548
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000C1368 File Offset: 0x000BF568
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x000C1374 File Offset: 0x000BF574
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x000C1388 File Offset: 0x000BF588
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x000C13E3 File Offset: 0x000BF5E3
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			if (t == null)
			{
				return new ThingDefCount(null, 0);
			}
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x0400131C RID: 4892
		private ThingDef thingDef;

		// Token: 0x0400131D RID: 4893
		private int count;
	}
}
