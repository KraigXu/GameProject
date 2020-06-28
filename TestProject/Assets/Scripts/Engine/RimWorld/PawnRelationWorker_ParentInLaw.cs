using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B62 RID: 2914
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x06004448 RID: 17480 RVA: 0x00171224 File Offset: 0x0016F424
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (me.GetSpouse() == null)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
			return !worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other);
		}
	}
}
