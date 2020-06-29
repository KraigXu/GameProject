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

		
		
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_FromQuality";
			}
		}
	}
}
