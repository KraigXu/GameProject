using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5D RID: 2909
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x06004439 RID: 17465 RVA: 0x00170E50 File Offset: 0x0016F050
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && ((me.GetMother() != null && me.GetMother() == other.GetMother()) || (me.GetFather() != null && me.GetFather() == other.GetFather()));
		}
	}
}
