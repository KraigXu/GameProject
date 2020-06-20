using System;

namespace Verse
{
	// Token: 0x020003DE RID: 990
	public struct LabelValue
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x000B493A File Offset: 0x000B2B3A
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x000B4942 File Offset: 0x000B2B42
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000B494A File Offset: 0x000B2B4A
		public LabelValue(string label, string value)
		{
			this = default(LabelValue);
			this.label = label;
			this.value = value;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000B493A File Offset: 0x000B2B3A
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x040011D4 RID: 4564
		private string label;

		// Token: 0x040011D5 RID: 4565
		private string value;
	}
}
