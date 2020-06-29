using System;

namespace Verse
{
	
	public class BodyPartGroupDef : Def
	{
		
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

		
		[MustTranslate]
		public string labelShort;

		
		public int listOrder;

		
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
