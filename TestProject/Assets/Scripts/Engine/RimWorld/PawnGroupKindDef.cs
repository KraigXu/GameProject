using System;
using Verse;

namespace RimWorld
{
	
	public class PawnGroupKindDef : Def
	{
		
		
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
