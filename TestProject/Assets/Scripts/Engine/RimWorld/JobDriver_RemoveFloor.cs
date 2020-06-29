using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		
		// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000FAF75 File Offset: 0x000F9175
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000FAF7C File Offset: 0x000F917C
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x000FAF83 File Offset: 0x000F9183
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		
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
