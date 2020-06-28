using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AF RID: 431
	public abstract class PlaceWorker
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsBuildDesignatorVisible(BuildableDef def)
		{
			return true;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00044240 File Offset: 0x00042440
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00044247 File Offset: 0x00042447
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}
	}
}
