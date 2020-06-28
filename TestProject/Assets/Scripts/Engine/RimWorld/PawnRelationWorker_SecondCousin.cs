using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B63 RID: 2915
	public class PawnRelationWorker_SecondCousin : PawnRelationWorker
	{
		// Token: 0x0600444A RID: 17482 RVA: 0x00171268 File Offset: 0x0016F468
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			Pawn mother = other.GetMother();
			if (mother != null && ((mother.GetMother() != null && worker.InRelation(me, mother.GetMother())) || (mother.GetFather() != null && worker.InRelation(me, mother.GetFather()))))
			{
				return true;
			}
			Pawn father = other.GetFather();
			return father != null && ((father.GetMother() != null && worker.InRelation(me, father.GetMother())) || (father.GetFather() != null && worker.InRelation(me, father.GetFather())));
		}
	}
}
