using System;

namespace Verse
{
	// Token: 0x0200041A RID: 1050
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x000C10E9 File Offset: 0x000BF2E9
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x000C10F4 File Offset: 0x000BF2F4
		public int Count
		{
			get
			{
				return this.countInt;
			}
			set
			{
				if (value < 0)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to set ThingCountClass stack count to ",
						value,
						". thing=",
						this.thing
					}), false);
					this.countInt = 0;
					return;
				}
				if (this.thing != null && value > this.thing.stackCount)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to set ThingCountClass stack count to ",
						value,
						", but thing's stack count is only ",
						this.thing.stackCount,
						". thing=",
						this.thing
					}), false);
					this.countInt = this.thing.stackCount;
					return;
				}
				this.countInt = value;
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public ThingCountClass()
		{
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000C11BD File Offset: 0x000BF3BD
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000C11D3 File Offset: 0x000BF3D3
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000C11F8 File Offset: 0x000BF3F8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.Count,
				"x ",
				(this.thing != null) ? this.thing.LabelShort : "null",
				")"
			});
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000C1253 File Offset: 0x000BF453
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x0400131A RID: 4890
		public Thing thing;

		// Token: 0x0400131B RID: 4891
		private int countInt;
	}
}
