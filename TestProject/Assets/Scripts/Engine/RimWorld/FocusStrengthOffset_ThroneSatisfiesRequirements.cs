using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D26 RID: 3366
	public class FocusStrengthOffset_ThroneSatisfiesRequirements : FocusStrengthOffset
	{
		// Token: 0x060051DB RID: 20955 RVA: 0x001B60EC File Offset: 0x001B42EC
		public override string GetExplanation(Thing parent)
		{
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x001B60F5 File Offset: 0x001B42F5
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_SatisfiesTitle".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x001B6125 File Offset: 0x001B4325
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (!this.CanApply(parent, user))
			{
				return 0f;
			}
			return this.offset;
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x001B6140 File Offset: 0x001B4340
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (user == null)
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = user.royalty;
			bool? flag = (royalty != null) ? new bool?(royalty.GetUnmetThroneroomRequirements(true, false).Any<string>()) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}
	}
}
