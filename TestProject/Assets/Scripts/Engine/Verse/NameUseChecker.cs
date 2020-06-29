using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public static class NameUseChecker
	{
		
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0003AB7C File Offset: 0x00038D7C
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					if (pawn.Name != null)
					{
						yield return pawn.Name;
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		
		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = name as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = name as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		
		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				NameSingle nameSingle = pawn.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}
	}
}
