using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A56 RID: 2646
	public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
	{
		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003E92 RID: 16018 RVA: 0x0014BD58 File Offset: 0x00149F58
		public override int SeedPart
		{
			get
			{
				return 920906419;
			}
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x0014BD60 File Offset: 0x00149F60
		public override void Generate(Map map, GenStepParams parms)
		{
			this.minSpacing = 5f;
			this.warnOnFail = false;
			int num = base.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0014BDC0 File Offset: 0x00149FC0
		protected ThingDef ChooseThingDef()
		{
			if (this.forcedDefToScatter != null)
			{
				return this.forcedDefToScatter;
			}
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeightWithFallback(delegate(ThingDef d)
			{
				if (d.building == null)
				{
					return 0f;
				}
				if (d.building.mineableThing != null && d.building.mineableThing.BaseMarketValue > this.maxValue)
				{
					return 0f;
				}
				return d.building.mineableScatterCommonality;
			}, null);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x0014BDE8 File Offset: 0x00149FE8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (base.NearUsedSpot(c, this.minSpacing))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x0014BE28 File Offset: 0x0014A028
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			ThingDef thingDef = this.ChooseThingDef();
			if (thingDef == null)
			{
				return;
			}
			int numCells = (this.forcedLumpSize > 0) ? this.forcedLumpSize : thingDef.building.mineableScatterLumpSizeRange.RandomInRange;
			this.recentLumpCells.Clear();
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(c, map, numCells))
			{
				GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
				this.recentLumpCells.Add(intVec);
			}
		}

		// Token: 0x04002472 RID: 9330
		public ThingDef forcedDefToScatter;

		// Token: 0x04002473 RID: 9331
		public int forcedLumpSize;

		// Token: 0x04002474 RID: 9332
		public float maxValue = float.MaxValue;

		// Token: 0x04002475 RID: 9333
		[Unsaved(false)]
		protected List<IntVec3> recentLumpCells = new List<IntVec3>();
	}
}
