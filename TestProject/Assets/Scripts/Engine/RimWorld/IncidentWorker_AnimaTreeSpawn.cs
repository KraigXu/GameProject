using System;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_AnimaTreeSpawn : IncidentWorker
	{
		
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			int num = GenStep_AnimaTrees.DesiredTreeCountForMap(map);
			IntVec3 intVec;
			return map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeAnima).Count < num && this.TryFindRootCell(map, out intVec);
		}

		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 cell;
			if (!this.TryFindRootCell(map, out cell))
			{
				return false;
			}
			Thing t;
			if (!GenStep_AnimaTrees.TrySpawnAt(cell, map, 0.05f, out t))
			{
				return false;
			}
			if (PawnsFinder.HomeMaps_FreeColonistsSpawned.Any((Pawn c) => c.HasPsylink && MeditationFocusDefOf.Natural.CanPawnUse(c)))
			{
				base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			}
			return true;
		}

		
		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => GenStep_AnimaTrees.CanSpawnAt(x, map, 40, 0, 22, 10), map, out cell) || CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => GenStep_AnimaTrees.CanSpawnAt(x, map, 10, 0, 18, 20), map, out cell);
		}
	}
}
