using System;
using Verse;

namespace RimWorld
{
	
	public class Verb_BeatFire : Verb
	{
		
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		
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

		
		private const int DamageAmount = 32;
	}
}
