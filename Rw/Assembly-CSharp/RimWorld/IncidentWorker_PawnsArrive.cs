using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F9 RID: 2553
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		// Token: 0x06003CB7 RID: 15543 RVA: 0x00140D3C File Offset: 0x0013EF3C
		protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, map, desperate)
			select f;
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x00140D80 File Offset: 0x0013EF80
		protected virtual bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return !f.IsPlayer && !f.defeated && (desperate || (f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp)));
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x00140DE4 File Offset: 0x0013EFE4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return parms.faction != null || this.CandidateFactions(map, false).Any<Faction>();
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00140E14 File Offset: 0x0013F014
		public string DebugListingOfGroupSources()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				stringBuilder.Append(faction.Name);
				if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, false))
				{
					stringBuilder.Append("    YES");
				}
				else if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, true))
				{
					stringBuilder.Append("    YES-DESPERATE");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
