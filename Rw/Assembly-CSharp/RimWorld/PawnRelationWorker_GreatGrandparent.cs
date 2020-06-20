using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5C RID: 2908
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x06004437 RID: 17463 RVA: 0x00170E37 File Offset: 0x0016F037
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
