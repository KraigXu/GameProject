using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x000E1BAC File Offset: 0x000DFDAC
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, Array.Empty<object>());
		}

		
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		
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

		
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		
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

		
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
