using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		
		
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		
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
