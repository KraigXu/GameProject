using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200053A RID: 1338
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x000E1BAC File Offset: 0x000DFDAC
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x17000780 RID: 1920
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x000E1BC8 File Offset: 0x000DFDC8
		public bool AnyPlayerForced
		{
			get
			{
				for (int i = 0; i < this.jobs.Count; i++)
				{
					if (this.jobs[i].job.playerForced)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000E1C06 File Offset: 0x000DFE06
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000E1C1E File Offset: 0x000DFE1E
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000E1C33 File Offset: 0x000DFE33
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000E1C48 File Offset: 0x000DFE48
		public bool Contains(Job j)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job == j)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000E1C84 File Offset: 0x000DFE84
		public QueuedJob Extract(Job j)
		{
			int num = this.jobs.FindIndex((QueuedJob qj) => qj.job == j);
			if (num >= 0)
			{
				QueuedJob result = this.jobs[num];
				this.jobs.RemoveAt(num);
				return result;
			}
			return null;
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000E1CD4 File Offset: 0x000DFED4
		public QueuedJob Dequeue()
		{
			if (this.jobs.NullOrEmpty<QueuedJob>())
			{
				return null;
			}
			QueuedJob result = this.jobs[0];
			this.jobs.RemoveAt(0);
			return result;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000E1CFD File Offset: 0x000DFEFD
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000E1D0C File Offset: 0x000DFF0C
		public bool AnyCanBeginNow(Pawn pawn, bool whileLyingDown)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job.CanBeginNow(pawn, whileLyingDown))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000E1D4C File Offset: 0x000DFF4C
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000E1D4C File Offset: 0x000DFF4C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x0400171A RID: 5914
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
