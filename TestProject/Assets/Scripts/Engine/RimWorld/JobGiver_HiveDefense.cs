using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000697 RID: 1687
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		// Token: 0x06002DE6 RID: 11750 RVA: 0x00102510 File Offset: 0x00100710
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x00102550 File Offset: 0x00100750
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x00102562 File Offset: 0x00100762
		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
