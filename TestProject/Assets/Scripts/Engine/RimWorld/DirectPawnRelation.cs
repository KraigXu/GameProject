using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC5 RID: 3013
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x0600473F RID: 18239 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public DirectPawnRelation()
		{
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x00181DA7 File Offset: 0x0017FFA7
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x00181DC4 File Offset: 0x0017FFC4
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}

		// Token: 0x04002908 RID: 10504
		public PawnRelationDef def;

		// Token: 0x04002909 RID: 10505
		public Pawn otherPawn;

		// Token: 0x0400290A RID: 10506
		public int startTicks;
	}
}
