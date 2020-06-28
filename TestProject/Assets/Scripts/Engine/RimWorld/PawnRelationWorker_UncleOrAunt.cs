using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B68 RID: 2920
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x0600445C RID: 17500 RVA: 0x00171B3C File Offset: 0x0016FD3C
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
