using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000486 RID: 1158
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000D1780 File Offset: 0x000CF980
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000D1790 File Offset: 0x000CF990
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

		// Token: 0x0600225A RID: 8794 RVA: 0x000D181B File Offset: 0x000CFA1B
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
