using System;

namespace Verse
{
	
	public class BodyPartGroupDef : Def
	{
		
		
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
