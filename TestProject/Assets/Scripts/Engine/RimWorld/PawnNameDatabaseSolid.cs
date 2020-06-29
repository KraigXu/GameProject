using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public static class PawnNameDatabaseSolid
	{
		
		static PawnNameDatabaseSolid()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
			}
		}

		
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		
		public static IEnumerable<NameTriple> AllNames()
		{
			foreach (KeyValuePair<GenderPossibility, List<NameTriple>> keyValuePair in PawnNameDatabaseSolid.solidNames)
			{
				foreach (NameTriple nameTriple in keyValuePair.Value)
				{
					yield return nameTriple;
				}
				List<NameTriple>.Enumerator enumerator2 = default(List<NameTriple>.Enumerator);
			}
			Dictionary<GenderPossibility, List<NameTriple>>.Enumerator enumerator = default(Dictionary<GenderPossibility, List<NameTriple>>.Enumerator);
			yield break;
			yield break;
		}

		
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		
		private const float PreferredNameChance = 0.5f;
	}
}
