using System;

namespace Verse.AI
{
	// Token: 0x02000586 RID: 1414
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		// Token: 0x0600284A RID: 10314 RVA: 0x000EE77C File Offset: 0x000EC97C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 2500)
			{
				return ThinkResult.NoJob;
			}
			this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
			float num = this.MtbHours(pawn);
			if (num <= 0f)
			{
				return ThinkResult.NoJob;
			}
			Rand.PushState();
			int salt = Gen.HashCombineInt(base.UniqueSaveKey, 26504059);
			Rand.Seed = pawn.RandSeedForHour(salt);
			bool flag = Rand.MTBEventOccurs(num, 2500f, 2500f);
			Rand.PopState();
			if (flag)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x0600284B RID: 10315
		protected abstract float MtbHours(Pawn pawn);

		// Token: 0x0600284C RID: 10316 RVA: 0x000EE818 File Offset: 0x000ECA18
		private int GetLastTryTick(Pawn pawn)
		{
			int result;
			if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out result))
			{
				return result;
			}
			return -99999;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000EE846 File Offset: 0x000ECA46
		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
