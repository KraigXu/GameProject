using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D94 RID: 3476
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06005498 RID: 21656 RVA: 0x001C3647 File Offset: 0x001C1847
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x001C3654 File Offset: 0x001C1854
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			Faction faction = (pawn != null) ? pawn.FactionOrExtraHomeFaction : target.Faction;
			if (user.Faction != null && faction != null && !faction.HostileTo(user.Faction))
			{
				faction.TryAffectGoodwillWith(user.Faction, this.PropsGoodwillImpact.goodwillImpact, true, true, "GoodwillChangedReason_UsedItem".Translate(this.parent.LabelShort, target.LabelShort, this.parent.Named("ITEM"), target.Named("TARGET")), new GlobalTargetInfo?(target));
			}
		}
	}
}
