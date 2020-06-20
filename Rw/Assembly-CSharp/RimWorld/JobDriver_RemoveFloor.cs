using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200062D RID: 1581
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000FAF75 File Offset: 0x000F9175
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000FAF7C File Offset: 0x000F917C
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x000FAF83 File Offset: 0x000F9183
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000FAF8A File Offset: 0x000F918A
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				base.Map.terrainGrid.RemoveTopLayer(base.TargetLocA, true);
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
