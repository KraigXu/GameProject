using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B57 RID: 2903
	public class PawnRelationWorker_Grandchild : PawnRelationWorker
	{
		// Token: 0x0600442D RID: 17453 RVA: 0x00170CCC File Offset: 0x0016EECC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
