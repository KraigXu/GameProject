using System;
using Verse;

namespace RimWorld
{
	
	public class PawnGroupKindDef : Def
	{
		
		// (get) Token: 0x06003696 RID: 13974 RVA: 0x00127B98 File Offset: 0x00125D98
		public PawnGroupKindWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnGroupKindWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public Type workerClass = typeof(PawnGroupKindWorker);

		
		[Unsaved(false)]
		private PawnGroupKindWorker workerInt;
	}
}
