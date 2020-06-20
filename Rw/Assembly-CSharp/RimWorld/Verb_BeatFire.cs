using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200102E RID: 4142
	public class Verb_BeatFire : Verb
	{
		// Token: 0x0600631C RID: 25372 RVA: 0x00227135 File Offset: 0x00225335
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x0022714C File Offset: 0x0022534C
		protected override bool TryCastShot()
		{
			Fire fire = (Fire)this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			if (casterPawn.stances.FullBodyBusy || fire.TicksSinceSpawn == 0)
			{
				return false;
			}
			fire.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 32f, 0f, -1f, this.caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			casterPawn.Drawer.Notify_MeleeAttackOn(fire);
			return true;
		}

		// Token: 0x04003C4C RID: 15436
		private const int DamageAmount = 32;
	}
}
