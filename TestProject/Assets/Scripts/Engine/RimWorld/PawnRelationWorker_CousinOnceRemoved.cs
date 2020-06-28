using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B53 RID: 2899
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x0600441D RID: 17437 RVA: 0x00170A48 File Offset: 0x0016EC48
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
			if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
			{
				return true;
			}
			PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			return (other.GetMother() != null && worker2.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker2.InRelation(me, other.GetFather()));
		}
	}
}
