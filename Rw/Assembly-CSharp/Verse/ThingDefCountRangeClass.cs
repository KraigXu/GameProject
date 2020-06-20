using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200041E RID: 1054
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000C178B File Offset: 0x000BF98B
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x000C1798 File Offset: 0x000BF998
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000C17A5 File Offset: 0x000BF9A5
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000C17B2 File Offset: 0x000BF9B2
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000C17BF File Offset: 0x000BF9BF
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000C17CF File Offset: 0x000BF9CF
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x000C17E8 File Offset: 0x000BF9E8
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x000C1820 File Offset: 0x000BFA20
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.countRange = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000C187C File Offset: 0x000BFA7C
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

		// Token: 0x06001FA3 RID: 8099 RVA: 0x000C18D7 File Offset: 0x000BFAD7
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x000C18EC File Offset: 0x000BFAEC
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000C1908 File Offset: 0x000BFB08
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		// Token: 0x04001322 RID: 4898
		public ThingDef thingDef;

		// Token: 0x04001323 RID: 4899
		public IntRange countRange;
	}
}
