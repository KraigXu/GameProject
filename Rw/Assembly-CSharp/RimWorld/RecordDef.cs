using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F3 RID: 2291
	public class RecordDef : Def
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x001283AD File Offset: 0x001265AD
		public RecordWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecordWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04001F67 RID: 8039
		public RecordType type;

		// Token: 0x04001F68 RID: 8040
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x04001F69 RID: 8041
		public List<JobDef> measuredTimeJobs;

		// Token: 0x04001F6A RID: 8042
		[Unsaved(false)]
		private RecordWorker workerInt;
	}
}
