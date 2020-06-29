using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class RoyalTitleInheritanceWorker
	{
		
		public Pawn FindHeir(Faction faction, Pawn pawn, RoyalTitleDef title)
		{
			//RoyalTitleInheritanceWorker.c__DisplayClass1_0 c__DisplayClass1_ = new RoyalTitleInheritanceWorker.c__DisplayClass1_0();
			//c__DisplayClass1_.faction = faction;
			//c__DisplayClass1_.pawn = pawn;
			//c__DisplayClass1_.relatedPawns = new List<Pawn>();
			//foreach (Pawn pawn2 in c__DisplayClass1_.pawn.relations.RelatedPawns)
			//{
			//	if (!pawn2.Dead)
			//	{
			//		c__DisplayClass1_.relatedPawns.Add(pawn2);
			//	}
			//}
			//Pawn pawn3 = c__DisplayClass1_.<FindHeir>g__GetClosestFamilyPawn|0(false);
			//if (pawn3 != null)
			//{
			//	return pawn3;
			//}
			//Pawn pawn4 = (from p in PawnsFinder.AllMapsAndWorld_Alive
			//where p != c__DisplayClass1_.pawn && p.Faction == c__DisplayClass1_.pawn.Faction && p.RaceProps.Humanlike
			//select p).MaxByWithFallback((Pawn p) => c__DisplayClass1_.pawn.relations.OpinionOf(p), null);
			//if (pawn4 != null)
			//{
			//	return pawn4;
			//}
			//pawn3 = c__DisplayClass1_.<FindHeir>g__GetClosestFamilyPawn|0(true);
			//if (pawn3 != null)
			//{
			//	return pawn3;
			//}
			return null;
		}

		
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
