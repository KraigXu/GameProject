using System;

namespace Verse
{
	// Token: 0x02000418 RID: 1048
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000C0E6E File Offset: 0x000BF06E
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001F59 RID: 8025 RVA: 0x000C0E76 File Offset: 0x000BF076
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000C0E80 File Offset: 0x000BF080
		public ThingCount(Thing thing, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingCount stack count to ",
					count,
					". thing=",
					thing
				}), false);
				count = 0;
			}
			if (count > thing.stackCount)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingCount stack count to ",
					count,
					", but thing's stack count is only ",
					thing.stackCount,
					". thing=",
					thing
				}), false);
				count = thing.stackCount;
			}
			this.thing = thing;
			this.count = count;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000C0F25 File Offset: 0x000BF125
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000C0F4A File Offset: 0x000BF14A
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000C0F58 File Offset: 0x000BF158
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000C0F70 File Offset: 0x000BF170
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000C0F7E File Offset: 0x000BF17E
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000C0F9E File Offset: 0x000BF19E
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000C0FAA File Offset: 0x000BF1AA
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000C0FC0 File Offset: 0x000BF1C0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000C101B File Offset: 0x000BF21B
		public static implicit operator ThingCount(ThingCountClass t)
		{
			if (t == null)
			{
				return new ThingCount(null, 0);
			}
			return new ThingCount(t.thing, t.Count);
		}

		// Token: 0x04001318 RID: 4888
		private Thing thing;

		// Token: 0x04001319 RID: 4889
		private int count;
	}
}
