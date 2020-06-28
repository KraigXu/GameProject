using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200062E RID: 1582
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002B56 RID: 11094 RVA: 0x000FAFCA File Offset: 0x000F91CA
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x000FAFD1 File Offset: 0x000F91D1
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000FAFD8 File Offset: 0x000F91D8
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000FAFDF File Offset: 0x000F91DF
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000FAFF0 File Offset: 0x000F91F0
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
