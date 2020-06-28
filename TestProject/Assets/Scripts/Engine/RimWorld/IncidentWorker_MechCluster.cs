using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E8 RID: 2536
	public class IncidentWorker_MechCluster : IncidentWorker
	{
		// Token: 0x06003C67 RID: 15463 RVA: 0x0013F2C8 File Offset: 0x0013D4C8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.points, map, true);
			IntVec3 center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0.5f);
			if (!center.IsValid)
			{
				return false;
			}
			IEnumerable<Thing> targets = from t in MechClusterUtility.SpawnCluster(center, map, sketch, true, true, parms.questTag)
			where t.def != ThingDefOf.Wall && t.def != ThingDefOf.Barricade
			select t;
			base.SendStandardLetter(parms, new LookTargets(targets), Array.Empty<NamedArgument>());
			return true;
		}
	}
}
