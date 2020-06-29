using System;
using RimWorld;

namespace Verse
{
	
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x000C1268 File Offset: 0x000BF468
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x000C1270 File Offset: 0x000BF470
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x000C1278 File Offset: 0x000BF478
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x000C128C File Offset: 0x000BF48C
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		
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

		
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			if (t == null)
			{
				return new ThingDefCount(null, 0);
			}
			return new ThingDefCount(t.thingDef, t.count);
		}

		
		private ThingDef thingDef;

		
		private int count;
	}
}
