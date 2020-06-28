using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B66 RID: 2918
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x06004458 RID: 17496 RVA: 0x00171A90 File Offset: 0x0016FC90
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
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return !worker.InRelation(me, other) && worker.InRelation(me.GetSpouse(), other);
		}
	}
}
