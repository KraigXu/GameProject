using System;

namespace Verse.AI
{
	
	public class QueuedJob : IExposable
	{
		
		public QueuedJob()
		{
		}

		
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", Array.Empty<object>());
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}

		
		public Job job;

		
		public JobTag? tag;
	}
}
