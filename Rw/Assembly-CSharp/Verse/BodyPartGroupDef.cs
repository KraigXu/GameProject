using System;

namespace Verse
{
	// Token: 0x02000077 RID: 119
	public class BodyPartGroupDef : Def
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x000174C5 File Offset: 0x000156C5
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x000174E1 File Offset: 0x000156E1
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return base.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x040001A8 RID: 424
		[MustTranslate]
		public string labelShort;

		// Token: 0x040001A9 RID: 425
		public int listOrder;

		// Token: 0x040001AA RID: 426
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
