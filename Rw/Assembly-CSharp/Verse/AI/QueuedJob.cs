using System;

namespace Verse.AI
{
	// Token: 0x02000539 RID: 1337
	public class QueuedJob : IExposable
	{
		// Token: 0x06002642 RID: 9794 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public QueuedJob()
		{
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000E1B5A File Offset: 0x000DFD5A
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000E1B70 File Offset: 0x000DFD70
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", Array.Empty<object>());
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04001718 RID: 5912
		public Job job;

		// Token: 0x04001719 RID: 5913
		public JobTag? tag;
	}
}
