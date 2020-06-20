using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001079 RID: 4217
	public class HediffComp_ExplodeOnDeath : HediffComp
	{
		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x06006421 RID: 25633 RVA: 0x0022B0CA File Offset: 0x002292CA
		public HediffCompProperties_ExplodeOnDeath Props
		{
			get
			{
				return (HediffCompProperties_ExplodeOnDeath)this.props;
			}
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x0022B0D8 File Offset: 0x002292D8
		public override void Notify_PawnKilled()
		{
			GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, this.Props.explosionRadius, this.Props.damageDef, base.Pawn, this.Props.damageAmount, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
			if (this.Props.destroyGear)
			{
				base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x0022B17E File Offset: 0x0022937E
		public override void Notify_PawnDied()
		{
			if (this.Props.destroyBody)
			{
				base.Pawn.Corpse.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
