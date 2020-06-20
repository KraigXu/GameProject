using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B0C RID: 2828
	public struct ThoughtToAddToAll
	{
		// Token: 0x0600429B RID: 17051 RVA: 0x00163D10 File Offset: 0x00161F10
		public ThoughtToAddToAll(ThoughtDef thoughtDef, Pawn otherPawn = null)
		{
			if (thoughtDef == null)
			{
				throw new NullReferenceException("Thought def cant be null!");
			}
			this.thoughtDef = thoughtDef;
			this.otherPawn = otherPawn;
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00163D30 File Offset: 0x00161F30
		public void Add(Pawn to)
		{
			if (to.needs == null || to.needs.mood == null)
			{
				return;
			}
			if (this.otherPawn == null)
			{
				to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, null);
				return;
			}
			to.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, this.otherPawn);
		}

		// Token: 0x04002644 RID: 9796
		public ThoughtDef thoughtDef;

		// Token: 0x04002645 RID: 9797
		public Pawn otherPawn;
	}
}
