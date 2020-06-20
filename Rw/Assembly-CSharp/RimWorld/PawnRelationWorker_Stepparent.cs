using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B67 RID: 2919
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x0600445A RID: 17498 RVA: 0x00171AD4 File Offset: 0x0016FCD4
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
			PawnRelationWorker worker = PawnRelationDefOf.Spouse.Worker;
			return (me.GetMother() != null && worker.InRelation(me.GetMother(), other)) || (me.GetFather() != null && worker.InRelation(me.GetFather(), other));
		}
	}
}
