using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E4 RID: 2532
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x06003C5A RID: 15450 RVA: 0x0013ECC0 File Offset: 0x0013CEC0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && HiveUtility.TotalSpawnedHivesCount(map) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0013ECF8 File Offset: 0x0013CEF8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = InfestationUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, false, null);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x04002387 RID: 9095
		public const float HivePoints = 220f;
	}
}
