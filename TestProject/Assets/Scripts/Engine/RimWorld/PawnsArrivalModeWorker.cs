using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008ED RID: 2285
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x060036B0 RID: 14000 RVA: 0x00127F40 File Offset: 0x00126140
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return (parms.faction == null || this.def.minTechLevel == TechLevel.Undefined || parms.faction.def.techLevel >= this.def.minTechLevel) && (!parms.raidArrivalModeForQuickMilitaryAid || this.def.forQuickMilitaryAid) && (parms.raidStrategy == null || parms.raidStrategy.arriveModes.Contains(this.def));
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x00127FBB File Offset: 0x001261BB
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			if (this.def.selectionWeightCurve != null)
			{
				return this.def.selectionWeightCurve.Evaluate(parms.points);
			}
			return 0f;
		}

		// Token: 0x060036B2 RID: 14002
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x060036B3 RID: 14003 RVA: 0x00127FE6 File Offset: 0x001261E6
		public virtual void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x060036B4 RID: 14004
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);

		// Token: 0x04001F45 RID: 8005
		public PawnsArrivalModeDef def;
	}
}
