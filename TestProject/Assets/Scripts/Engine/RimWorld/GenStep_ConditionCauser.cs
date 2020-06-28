using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A66 RID: 2662
	public class GenStep_ConditionCauser : GenStep_Scatterer
	{
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x0014E131 File Offset: 0x0014C331
		public override int SeedPart
		{
			get
			{
				return 1068345639;
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0014E138 File Offset: 0x0014C338
		public override void Generate(Map map, GenStepParams parms)
		{
			this.currentParams = parms;
			this.count = 1;
			base.Generate(map, parms);
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0014E150 File Offset: 0x0014C350
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			CellRect rect = CellRect.CenteredOn(loc, 10, 10).ClipInsideMap(map);
			SitePart sitePart = this.currentParams.sitePart;
			sitePart.conditionCauserWasSpawned = true;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.conditionCauser = sitePart.conditionCauser;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("conditionCauserRoom", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04002497 RID: 9367
		private const int Size = 10;

		// Token: 0x04002498 RID: 9368
		private GenStepParams currentParams;
	}
}
