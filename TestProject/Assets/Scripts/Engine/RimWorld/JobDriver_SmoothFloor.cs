using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		
		// (get) Token: 0x06002B56 RID: 11094 RVA: 0x000FAFCA File Offset: 0x000F91CA
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x000FAFD1 File Offset: 0x000F91D1
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000FAFD8 File Offset: 0x000F91D8
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
