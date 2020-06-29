using System;
using Verse;

namespace RimWorld
{
	
	public class FocusStrengthOffset_Quality : FocusStrengthOffset_Curve
	{
		
		protected override float SourceValue(Thing parent)
		{
			QualityCategory qualityCategory;
			parent.TryGetQuality(out qualityCategory);
			return (float)qualityCategory;
		}

		
		public override float MaxOffset(bool forAbstract = false)
		{
			if (!forAbstract)
			{
				return 0f;
			}
			return base.MaxOffset(true);
		}

		
		// (get) Token: 0x060051D6 RID: 20950 RVA: 0x001B60AA File Offset: 0x001B42AA
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_FromQuality";
			}
		}
	}
}
