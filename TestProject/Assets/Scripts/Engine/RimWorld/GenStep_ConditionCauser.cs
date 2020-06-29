using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class GenStep_ConditionCauser : GenStep_Scatterer
	{
		
		// (get) Token: 0x06003ED7 RID: 16087 RVA: 0x0014E131 File Offset: 0x0014C331
		public override int SeedPart
		{
			get
			{
				return 1068345639;
			}
		}

		
		public override void Generate(Map map, GenStepParams parms)
		{
			this.currentParams = parms;
			this.count = 1;
			base.Generate(map, parms);
		}

		
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

		
		private const int Size = 10;

		
		private GenStepParams currentParams;
	}
}
