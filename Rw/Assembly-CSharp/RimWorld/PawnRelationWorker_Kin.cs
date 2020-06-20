using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5E RID: 2910
	public class PawnRelationWorker_Kin : PawnRelationWorker
	{
		// Token: 0x0600443B RID: 17467 RVA: 0x00170EA8 File Offset: 0x0016F0A8
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other || me.RaceProps.Animal != other.RaceProps.Animal)
			{
				return false;
			}
			IEnumerable<Pawn> familyByBlood = me.relations.FamilyByBlood;
			HashSet<Pawn> hashSet = familyByBlood as HashSet<Pawn>;
			if (hashSet == null)
			{
				return familyByBlood.Contains(other);
			}
			return hashSet.Contains(other);
		}
	}
}
