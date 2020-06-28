using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1F RID: 2847
	public class PawnGroupMaker
	{
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x00168BBC File Offset: 0x00166DBC
		public float MinPointsToGenerateAnything
		{
			get
			{
				return this.kindDef.Worker.MinPointsToGenerateAnything(this);
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00168BCF File Offset: 0x00166DCF
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x00168BE4 File Offset: 0x00166DE4
		public IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			return this.kindDef.Worker.GeneratePawnKindsExample(parms, this);
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x00168BF8 File Offset: 0x00166DF8
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			return parms.points <= this.maxTotalPoints && (this.disallowedStrategies == null || !this.disallowedStrategies.Contains(parms.raidStrategy)) && this.kindDef.Worker.CanGenerateFrom(parms, this);
		}

		// Token: 0x0400267C RID: 9852
		public PawnGroupKindDef kindDef;

		// Token: 0x0400267D RID: 9853
		public float commonality = 100f;

		// Token: 0x0400267E RID: 9854
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x0400267F RID: 9855
		public float maxTotalPoints = 9999999f;

		// Token: 0x04002680 RID: 9856
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x04002681 RID: 9857
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x04002682 RID: 9858
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x04002683 RID: 9859
		public List<PawnGenOption> guards = new List<PawnGenOption>();
	}
}
