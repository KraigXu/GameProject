using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A6A RID: 2666
	public static class ManhunterPackGenStepUtility
	{
		// Token: 0x06003EE6 RID: 16102 RVA: 0x0014E66A File Offset: 0x0014C86A
		public static bool TryGetAnimalsKind(float points, int tile, out PawnKindDef animalKind)
		{
			return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, tile, out animalKind) || ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(points, -1, out animalKind);
		}
	}
}
