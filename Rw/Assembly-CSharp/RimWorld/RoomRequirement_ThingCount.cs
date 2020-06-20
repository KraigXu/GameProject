using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001042 RID: 4162
	public class RoomRequirement_ThingCount : RoomRequirement_Thing
	{
		// Token: 0x06006378 RID: 25464 RVA: 0x0022876E File Offset: 0x0022696E
		public override bool Met(Room r, Pawn p = null)
		{
			return this.Count(r) >= this.count;
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x00228782 File Offset: 0x00226982
		public int Count(Room r)
		{
			return r.ThingCount(this.thingDef);
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x00228790 File Offset: 0x00226990
		public override string Label(Room r = null)
		{
			bool flag = !this.labelKey.NullOrEmpty();
			string text = flag ? this.labelKey.Translate() : this.thingDef.label;
			if (r != null)
			{
				return string.Concat(new object[]
				{
					text,
					" ",
					this.Count(r),
					"/",
					this.count
				});
			}
			if (!flag)
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
			return text + " x" + this.count;
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x00228839 File Offset: 0x00226A39
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.count <= 0)
			{
				yield return "count must be larger than 0";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003C91 RID: 15505
		public int count;
	}
}
