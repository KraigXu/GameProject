using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D3 RID: 2515
	internal class IncidentWorker_Alphabeavers : IncidentWorker
	{
		// Token: 0x06003C12 RID: 15378 RVA: 0x0013D12C File Offset: 0x0013B32C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0013D160 File Offset: 0x0013B360
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef alphabeaver = PawnKindDefOf.Alphabeaver;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			float freeColonistsCount = (float)map.mapPawns.FreeColonistsCount;
			float randomInRange = IncidentWorker_Alphabeavers.CountPerColonistRange.RandomInRange;
			int num = Mathf.Clamp(GenMath.RoundRandom(freeColonistsCount * randomInRange), 1, 10);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(alphabeaver, null), loc, map, WipeMode.Vanish)).needs.food.CurLevelPercentage = 1f;
			}
			base.SendStandardLetter("LetterLabelBeaversArrived".Translate(), "BeaversArrived".Translate(), LetterDefOf.ThreatSmall, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04002371 RID: 9073
		private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 1.5f);

		// Token: 0x04002372 RID: 9074
		private const int MinCount = 1;

		// Token: 0x04002373 RID: 9075
		private const int MaxCount = 10;
	}
}
