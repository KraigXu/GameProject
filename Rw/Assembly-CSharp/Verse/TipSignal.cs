using System;

namespace Verse
{
	// Token: 0x020003BB RID: 955
	public struct TipSignal
	{
		// Token: 0x06001C24 RID: 7204 RVA: 0x000AB311 File Offset: 0x000A9511
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000AB33A File Offset: 0x000A953A
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000AB363 File Offset: 0x000A9563
		public TipSignal(string text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000AB39B File Offset: 0x000A959B
		public TipSignal(string text, float delay)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = delay;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000AB3D0 File Offset: 0x000A95D0
		public TipSignal(TaggedString text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text.Resolve();
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000AB42A File Offset: 0x000A962A
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x000AB457 File Offset: 0x000A9657
		public TipSignal(Func<string> textGetter, int uniqueId, TooltipPriority priority)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000AB484 File Offset: 0x000A9684
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
			this.delay = 0.45f;
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x000AB4BC File Offset: 0x000A96BC
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000AB4C4 File Offset: 0x000A96C4
		public static implicit operator TipSignal(TaggedString str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000AB4CC File Offset: 0x000A96CC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Tip(",
				this.text,
				", ",
				this.uniqueId,
				")"
			});
		}

		// Token: 0x0400109A RID: 4250
		public const float DefaultDelay = 0.45f;

		// Token: 0x0400109B RID: 4251
		public string text;

		// Token: 0x0400109C RID: 4252
		public Func<string> textGetter;

		// Token: 0x0400109D RID: 4253
		public int uniqueId;

		// Token: 0x0400109E RID: 4254
		public TooltipPriority priority;

		// Token: 0x0400109F RID: 4255
		public float delay;
	}
}
