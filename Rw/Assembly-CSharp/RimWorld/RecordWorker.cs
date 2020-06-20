using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008F5 RID: 2293
	public class RecordWorker
	{
		// Token: 0x060036D0 RID: 14032 RVA: 0x001283F8 File Offset: 0x001265F8
		public virtual bool ShouldMeasureTimeNow(Pawn pawn)
		{
			if (this.def.measuredTimeJobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob == null)
			{
				return false;
			}
			for (int i = 0; i < this.def.measuredTimeJobs.Count; i++)
			{
				if (curJob.def == this.def.measuredTimeJobs[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001F6F RID: 8047
		public RecordDef def;
	}
}
