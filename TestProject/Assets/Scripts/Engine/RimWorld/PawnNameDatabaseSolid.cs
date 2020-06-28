using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B75 RID: 2933
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x060044A7 RID: 17575 RVA: 0x0017316C File Offset: 0x0017136C
		static PawnNameDatabaseSolid()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
			}
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x001731E4 File Offset: 0x001713E4
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x001731F7 File Offset: 0x001713F7
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x00173204 File Offset: 0x00171404
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

		// Token: 0x04002737 RID: 10039
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04002738 RID: 10040
		private const float PreferredNameChance = 0.5f;
	}
}
