using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD1 RID: 2769
	public class CompAbilityEffect_Smokepop : CompAbilityEffect
	{
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x0015ED73 File Offset: 0x0015CF73
		public new CompProperties_AbilitySmokepop Props
		{
			get
			{
				return (CompProperties_AbilitySmokepop)this.props;
			}
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x0015ED80 File Offset: 0x0015CF80
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.smokeRadius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x0015EDEF File Offset: 0x0015CFEF
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(target.Cell, this.Props.smokeRadius);
		}
	}
}
