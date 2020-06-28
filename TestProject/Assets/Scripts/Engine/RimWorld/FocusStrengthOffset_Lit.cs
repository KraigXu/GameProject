using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D29 RID: 3369
	public class FocusStrengthOffset_Lit : FocusStrengthOffset
	{
		// Token: 0x060051E9 RID: 20969 RVA: 0x001B62F8 File Offset: 0x001B44F8
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Lit".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x001B6347 File Offset: 0x001B4547
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Lit".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x001B6125 File Offset: 0x001B4325
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (!this.CanApply(parent, user))
			{
				return 0f;
			}
			return this.offset;
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x001B6378 File Offset: 0x001B4578
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			CompGlower compGlower = parent.TryGetComp<CompGlower>();
			return compGlower != null && compGlower.Glows;
		}
	}
}
