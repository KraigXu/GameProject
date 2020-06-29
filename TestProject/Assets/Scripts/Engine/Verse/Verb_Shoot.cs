using System;
using RimWorld;

namespace Verse
{
	
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000D1780 File Offset: 0x000CF980
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			if (pawn != null && !pawn.Downed && this.CasterIsPawn && this.CasterPawn.skills != null)
			{
				float num = pawn.HostileTo(this.caster) ? 170f : 20f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
				this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
			}
		}

		
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag && this.CasterIsPawn)
			{
				this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			return flag;
		}
	}
}
