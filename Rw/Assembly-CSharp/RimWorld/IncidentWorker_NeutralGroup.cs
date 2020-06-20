using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FC RID: 2556
	public abstract class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
	{
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003CCF RID: 15567 RVA: 0x00141A54 File Offset: 0x0013FC54
		protected virtual PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Peaceful;
			}
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x00141A5C File Offset: 0x0013FC5C
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.def.hidden && !f.HostileTo(Faction.OfPlayer) && f.def.pawnGroupMakers != null && f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.PawnGroupKindDef) && !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, f);
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x00141AC5 File Offset: 0x0013FCC5
		protected bool TryResolveParms(IncidentParms parms)
		{
			if (!this.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			this.ResolveParmsPoints(parms);
			return true;
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x00141ADC File Offset: 0x0013FCDC
		protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return (parms.spawnCenter.IsValid || RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, false, null)) && (parms.faction != null || base.CandidateFactions(map, false).TryRandomElement(out parms.faction) || base.CandidateFactions(map, true).TryRandomElement(out parms.faction));
		}

		// Token: 0x06003CD3 RID: 15571
		protected abstract void ResolveParmsPoints(IncidentParms parms);

		// Token: 0x06003CD4 RID: 15572 RVA: 0x00141B50 File Offset: 0x0013FD50
		protected List<Pawn> SpawnPawns(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(this.PawnGroupKindDef, parms, true), false).ToList<Pawn>();
			foreach (Thing newThing in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
				GenSpawn.Spawn(newThing, loc, map, WipeMode.Vanish);
			}
			return list;
		}
	}
}
