using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D27 RID: 3367
	public class FocusStrengthOffset_GraveFull : FocusStrengthOffset
	{
		// Token: 0x060051E0 RID: 20960 RVA: 0x001B6190 File Offset: 0x001B4390
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				Building_Grave building_Grave = parent as Building_Grave;
				return "StatsReport_GraveFull".Translate(building_Grave.Corpse.InnerPawn.LabelShortCap) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001B61FB File Offset: 0x001B43FB
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveFullAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001B6125 File Offset: 0x001B4325
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			if (!this.CanApply(parent, user))
			{
				return 0f;
			}
			return this.offset;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x001B622C File Offset: 0x001B442C
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave;
			return parent.Spawned && (building_Grave = (parent as Building_Grave)) != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike;
		}
	}
}
