using System;
using System.Xml;

namespace Verse
{
	
	public sealed class ThingDefCountRangeClass : IExposable
	{
		
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000C178B File Offset: 0x000BF98B
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x000C1798 File Offset: 0x000BF998
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000C17A5 File Offset: 0x000BF9A5
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x000C17B2 File Offset: 0x000BF9B2
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		
		public ThingDefCountRangeClass()
		{
		}

		
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		
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

		
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}

		
		public ThingDef thingDef;

		
		public IntRange countRange;
	}
}
