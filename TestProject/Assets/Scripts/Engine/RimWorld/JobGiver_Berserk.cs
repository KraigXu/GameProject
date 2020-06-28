using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070B RID: 1803
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x06002F9F RID: 12191 RVA: 0x0010C46C File Offset: 0x0010A66C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.5f)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Combat);
				job.expiryInterval = 90;
				job.canUseRangedWeapon = false;
				return job;
			}
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				return null;
			}
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null)
			{
				Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn2);
				job2.maxNumMeleeAttacks = 1;
				job2.expiryInterval = Rand.Range(420, 900);
				job2.canBash = true;
				return job2;
			}
			return null;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x0010C4EC File Offset: 0x0010A6EC
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, delegate(Thing x)
			{
				Pawn pawn2;
				return (pawn2 = (x as Pawn)) != null && pawn2.Spawned && !pawn2.Downed && !pawn2.IsInvisible();
			}, 0f, 40f, default(IntVec3), float.MaxValue, true, true);
		}

		// Token: 0x04001ACA RID: 6858
		private const float MaxAttackDistance = 40f;

		// Token: 0x04001ACB RID: 6859
		private const float WaitChance = 0.5f;

		// Token: 0x04001ACC RID: 6860
		private const int WaitTicks = 90;

		// Token: 0x04001ACD RID: 6861
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04001ACE RID: 6862
		private const int MaxMeleeChaseTicks = 900;
	}
}
