using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B51 RID: 2897
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x06004419 RID: 17433 RVA: 0x001709B4 File Offset: 0x0016EBB4
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (other.GetSpouse() == null)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return !worker.InRelation(me, other) && worker.InRelation(me, other.GetSpouse());
		}
	}
}
