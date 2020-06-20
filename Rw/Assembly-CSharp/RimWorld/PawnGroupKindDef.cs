using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008E8 RID: 2280
	public class PawnGroupKindDef : Def
	{
		// Token: 0x170009C6 RID: 2502
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

		// Token: 0x04001F24 RID: 7972
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x04001F25 RID: 7973
		[Unsaved(false)]
		private PawnGroupKindWorker workerInt;
	}
}
