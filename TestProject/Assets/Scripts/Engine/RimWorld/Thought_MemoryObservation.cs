using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD0 RID: 3024
	public class Thought_MemoryObservation : Thought_Memory
	{
		// Token: 0x17000CC3 RID: 3267
		// (set) Token: 0x060047C3 RID: 18371 RVA: 0x001855A0 File Offset: 0x001837A0
		public Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x001855AE File Offset: 0x001837AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetThingID, "targetThingID", 0, false);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x001855C8 File Offset: 0x001837C8
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
			Thought_MemoryObservation thought_MemoryObservation = null;
			List<Thought_Memory> memories = thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemoryObservation thought_MemoryObservation2 = memories[i] as Thought_MemoryObservation;
				if (thought_MemoryObservation2 != null && thought_MemoryObservation2.def == this.def && thought_MemoryObservation2.targetThingID == this.targetThingID && (thought_MemoryObservation == null || thought_MemoryObservation2.age > thought_MemoryObservation.age))
				{
					thought_MemoryObservation = thought_MemoryObservation2;
				}
			}
			if (thought_MemoryObservation != null)
			{
				showBubble = (thought_MemoryObservation.age > thought_MemoryObservation.def.DurationTicks / 2);
				thought_MemoryObservation.Renew();
				return true;
			}
			showBubble = true;
			return false;
		}

		// Token: 0x04002937 RID: 10551
		private int targetThingID;
	}
}
