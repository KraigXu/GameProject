using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B59 RID: 2905
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x06004431 RID: 17457 RVA: 0x00170D6B File Offset: 0x0016EF6B
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
