using System;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_Smokepop : CompAbilityEffect
	{
		
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x0015ED73 File Offset: 0x0015CF73
		public new CompProperties_AbilitySmokepop Props
		{
			get
			{
				return (CompProperties_AbilitySmokepop)this.props;
			}
		}

		
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
		}
	}
}
