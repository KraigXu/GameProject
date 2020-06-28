using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B52 RID: 2898
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x0600441B RID: 17435 RVA: 0x001709F8 File Offset: 0x0016EBF8
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.UncleOrAunt.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
