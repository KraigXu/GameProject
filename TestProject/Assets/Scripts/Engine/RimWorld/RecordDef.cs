using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class RecordDef : Def
	{
		
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

		
		public RecordType type;

		
		public Type workerClass = typeof(RecordWorker);

		
		public List<JobDef> measuredTimeJobs;

		
		[Unsaved(false)]
		private RecordWorker workerInt;
	}
}
