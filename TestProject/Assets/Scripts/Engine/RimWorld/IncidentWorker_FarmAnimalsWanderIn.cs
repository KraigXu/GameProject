using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DF RID: 2527
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		// Token: 0x06003C46 RID: 15430 RVA: 0x0013E698 File Offset: 0x0013C898
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			PawnKindDef pawnKindDef;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null) && this.TryFindRandomPawnKind(map, out pawnKindDef);
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x0013E6D8 File Offset: 0x0013C8D8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			PawnKindDef pawnKindDef;
			if (!this.TryFindRandomPawnKind(map, out pawnKindDef))
			{
				return false;
			}
			int num = Mathf.Clamp(GenMath.RoundRandom(2.5f / pawnKindDef.RaceProps.baseBodySize), 2, 10);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 12, null);
				Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
				pawn.SetFaction(Faction.OfPlayer, null);
			}
			base.SendStandardLetter("LetterLabelFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst(), "LetterFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x0013E7C4 File Offset: 0x0013C9C4
		private bool TryFindRandomPawnKind(Map map, out PawnKindDef kind)
		{
			return (from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.RaceProps.wildness < 0.35f && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race)
			select x).TryRandomElementByWeight((PawnKindDef k) => 0.420000017f - k.RaceProps.wildness, out kind);
		}

		// Token: 0x04002383 RID: 9091
		private const float MaxWildness = 0.35f;

		// Token: 0x04002384 RID: 9092
		private const float TotalBodySizeToSpawn = 2.5f;
	}
}
