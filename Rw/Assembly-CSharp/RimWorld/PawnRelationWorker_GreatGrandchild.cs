using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5B RID: 2907
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x06004435 RID: 17461 RVA: 0x00170DE8 File Offset: 0x0016EFE8
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandchild.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
