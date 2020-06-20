using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D24 RID: 3364
	public class FocusStrengthOffset_Quality : FocusStrengthOffset_Curve
	{
		// Token: 0x060051D4 RID: 20948 RVA: 0x001B6080 File Offset: 0x001B4280
		protected override float SourceValue(Thing parent)
		{
			QualityCategory qualityCategory;
			parent.TryGetQuality(out qualityCategory);
			return (float)qualityCategory;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001B6098 File Offset: 0x001B4298
		public override float MaxOffset(bool forAbstract = false)
		{
			if (!forAbstract)
			{
				return 0f;
			}
			return base.MaxOffset(true);
		}

		// Token: 0x17000E6F RID: 3695
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
