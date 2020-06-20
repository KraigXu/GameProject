using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B60 RID: 2912
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		// Token: 0x06004441 RID: 17473 RVA: 0x00170F7C File Offset: 0x0016F17C
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Child.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Sibling.Worker;
			PawnRelationWorker worker2 = PawnRelationDefOf.HalfSibling.Worker;
			return (other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother()))) || (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather())));
		}
	}
}
