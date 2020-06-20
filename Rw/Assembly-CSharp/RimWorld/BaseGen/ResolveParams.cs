using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02001096 RID: 4246
	public struct ResolveParams
	{
		// Token: 0x060064B4 RID: 25780 RVA: 0x00230A51 File Offset: 0x0022EC51
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x060064B5 RID: 25781 RVA: 0x00230A61 File Offset: 0x0022EC61
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x060064B6 RID: 25782 RVA: 0x00230A6F File Offset: 0x0022EC6F
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x060064B7 RID: 25783 RVA: 0x00230A7E File Offset: 0x0022EC7E
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x060064B8 RID: 25784 RVA: 0x00230A8C File Offset: 0x0022EC8C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"rect=",
				this.rect,
				", faction=",
				(this.faction != null) ? this.faction.ToString() : "null",
				", custom=",
				(this.custom != null) ? this.custom.Count.ToString() : "null",
				", pawnGroupMakerParams=",
				(this.pawnGroupMakerParams != null) ? this.pawnGroupMakerParams.ToString() : "null",
				", pawnGroupKindDef=",
				(this.pawnGroupKindDef != null) ? this.pawnGroupKindDef.ToString() : "null",
				", roofDef=",
				(this.roofDef != null) ? this.roofDef.ToString() : "null",
				", noRoof=",
				(this.noRoof != null) ? this.noRoof.ToString() : "null",
				", addRoomCenterToRootsToUnfog=",
				(this.addRoomCenterToRootsToUnfog != null) ? this.addRoomCenterToRootsToUnfog.ToString() : "null",
				", singleThingToSpawn=",
				(this.singleThingToSpawn != null) ? this.singleThingToSpawn.ToString() : "null",
				", singleThingDef=",
				(this.singleThingDef != null) ? this.singleThingDef.ToString() : "null",
				", singleThingStuff=",
				(this.singleThingStuff != null) ? this.singleThingStuff.ToString() : "null",
				", singleThingStackCount=",
				(this.singleThingStackCount != null) ? this.singleThingStackCount.ToString() : "null",
				", skipSingleThingIfHasToWipeBuildingOrDoesntFit=",
				(this.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null) ? this.skipSingleThingIfHasToWipeBuildingOrDoesntFit.ToString() : "null",
				", spawnBridgeIfTerrainCantSupportThing=",
				(this.spawnBridgeIfTerrainCantSupportThing != null) ? this.spawnBridgeIfTerrainCantSupportThing.ToString() : "null",
				", singlePawnToSpawn=",
				(this.singlePawnToSpawn != null) ? this.singlePawnToSpawn.ToString() : "null",
				", singlePawnKindDef=",
				(this.singlePawnKindDef != null) ? this.singlePawnKindDef.ToString() : "null",
				", disableSinglePawn=",
				(this.disableSinglePawn != null) ? this.disableSinglePawn.ToString() : "null",
				", singlePawnLord=",
				(this.singlePawnLord != null) ? this.singlePawnLord.ToString() : "null",
				", singlePawnSpawnCellExtraPredicate=",
				(this.singlePawnSpawnCellExtraPredicate != null) ? this.singlePawnSpawnCellExtraPredicate.ToString() : "null",
				", singlePawnGenerationRequest=",
				(this.singlePawnGenerationRequest != null) ? this.singlePawnGenerationRequest.ToString() : "null",
				", postThingSpawn=",
				(this.postThingSpawn != null) ? this.postThingSpawn.ToString() : "null",
				", postThingGenerate=",
				(this.postThingGenerate != null) ? this.postThingGenerate.ToString() : "null",
				", mechanoidsCount=",
				(this.mechanoidsCount != null) ? this.mechanoidsCount.ToString() : "null",
				", hivesCount=",
				(this.hivesCount != null) ? this.hivesCount.ToString() : "null",
				", disableHives=",
				(this.disableHives != null) ? this.disableHives.ToString() : "null",
				", thingRot=",
				(this.thingRot != null) ? this.thingRot.ToString() : "null",
				", wallStuff=",
				(this.wallStuff != null) ? this.wallStuff.ToString() : "null",
				", chanceToSkipWallBlock=",
				(this.chanceToSkipWallBlock != null) ? this.chanceToSkipWallBlock.ToString() : "null",
				", floorDef=",
				(this.floorDef != null) ? this.floorDef.ToString() : "null",
				", chanceToSkipFloor=",
				(this.chanceToSkipFloor != null) ? this.chanceToSkipFloor.ToString() : "null",
				", filthDef=",
				(this.filthDef != null) ? this.filthDef.ToString() : "null",
				", filthDensity=",
				(this.filthDensity != null) ? this.filthDensity.ToString() : "null",
				", floorOnlyIfTerrainSupports=",
				(this.floorOnlyIfTerrainSupports != null) ? this.floorOnlyIfTerrainSupports.ToString() : "null",
				", allowBridgeOnAnyImpassableTerrain=",
				(this.allowBridgeOnAnyImpassableTerrain != null) ? this.allowBridgeOnAnyImpassableTerrain.ToString() : "null",
				", clearEdificeOnly=",
				(this.clearEdificeOnly != null) ? this.clearEdificeOnly.ToString() : "null",
				", clearFillageOnly=",
				(this.clearFillageOnly != null) ? this.clearFillageOnly.ToString() : "null",
				", clearRoof=",
				(this.clearRoof != null) ? this.clearRoof.ToString() : "null",
				", ancientCryptosleepCasketGroupID=",
				(this.ancientCryptosleepCasketGroupID != null) ? this.ancientCryptosleepCasketGroupID.ToString() : "null",
				", podContentsType=",
				(this.podContentsType != null) ? this.podContentsType.ToString() : "null",
				", thingSetMakerDef=",
				(this.thingSetMakerDef != null) ? this.thingSetMakerDef.ToString() : "null",
				", thingSetMakerParams=",
				(this.thingSetMakerParams != null) ? this.thingSetMakerParams.ToString() : "null",
				", stockpileConcreteContents=",
				(this.stockpileConcreteContents != null) ? this.stockpileConcreteContents.Count.ToString() : "null",
				", stockpileMarketValue=",
				(this.stockpileMarketValue != null) ? this.stockpileMarketValue.ToString() : "null",
				", innerStockpileSize=",
				(this.innerStockpileSize != null) ? this.innerStockpileSize.ToString() : "null",
				", edgeDefenseWidth=",
				(this.edgeDefenseWidth != null) ? this.edgeDefenseWidth.ToString() : "null",
				", edgeDefenseTurretsCount=",
				(this.edgeDefenseTurretsCount != null) ? this.edgeDefenseTurretsCount.ToString() : "null",
				", edgeDefenseMortarsCount=",
				(this.edgeDefenseMortarsCount != null) ? this.edgeDefenseMortarsCount.ToString() : "null",
				", edgeDefenseGuardsCount=",
				(this.edgeDefenseGuardsCount != null) ? this.edgeDefenseGuardsCount.ToString() : "null",
				", mortarDef=",
				(this.mortarDef != null) ? this.mortarDef.ToString() : "null",
				", pathwayFloorDef=",
				(this.pathwayFloorDef != null) ? this.pathwayFloorDef.ToString() : "null",
				", cultivatedPlantDef=",
				(this.cultivatedPlantDef != null) ? this.cultivatedPlantDef.ToString() : "null",
				", fillWithThingsPadding=",
				(this.fillWithThingsPadding != null) ? this.fillWithThingsPadding.ToString() : "null",
				", settlementPawnGroupPoints=",
				(this.settlementPawnGroupPoints != null) ? this.settlementPawnGroupPoints.ToString() : "null",
				", settlementPawnGroupSeed=",
				(this.settlementPawnGroupSeed != null) ? this.settlementPawnGroupSeed.ToString() : "null",
				", streetHorizontal=",
				(this.streetHorizontal != null) ? this.streetHorizontal.ToString() : "null",
				", edgeThingAvoidOtherEdgeThings=",
				(this.edgeThingAvoidOtherEdgeThings != null) ? this.edgeThingAvoidOtherEdgeThings.ToString() : "null",
				", edgeThingMustReachMapEdge=",
				(this.edgeThingMustReachMapEdge != null) ? this.edgeThingMustReachMapEdge.ToString() : "null",
				", allowPlacementOffEdge=",
				(this.allowPlacementOffEdge != null) ? this.allowPlacementOffEdge.ToString() : "null",
				", thrustAxis=",
				(this.thrustAxis != null) ? this.thrustAxis.ToString() : "null",
				", makeWarningLetter=",
				(this.makeWarningLetter != null) ? this.makeWarningLetter.ToString() : "null",
				", allowedMonumentThings=",
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null"
			});
		}

		// Token: 0x04003D51 RID: 15697
		public CellRect rect;

		// Token: 0x04003D52 RID: 15698
		public Faction faction;

		// Token: 0x04003D53 RID: 15699
		private Dictionary<string, object> custom;

		// Token: 0x04003D54 RID: 15700
		public PawnGroupMakerParms pawnGroupMakerParams;

		// Token: 0x04003D55 RID: 15701
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04003D56 RID: 15702
		public RoofDef roofDef;

		// Token: 0x04003D57 RID: 15703
		public bool? noRoof;

		// Token: 0x04003D58 RID: 15704
		public bool? addRoomCenterToRootsToUnfog;

		// Token: 0x04003D59 RID: 15705
		public Thing singleThingToSpawn;

		// Token: 0x04003D5A RID: 15706
		public ThingDef singleThingDef;

		// Token: 0x04003D5B RID: 15707
		public ThingDef singleThingStuff;

		// Token: 0x04003D5C RID: 15708
		public int? singleThingStackCount;

		// Token: 0x04003D5D RID: 15709
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		// Token: 0x04003D5E RID: 15710
		public bool? spawnBridgeIfTerrainCantSupportThing;

		// Token: 0x04003D5F RID: 15711
		public Pawn singlePawnToSpawn;

		// Token: 0x04003D60 RID: 15712
		public PawnKindDef singlePawnKindDef;

		// Token: 0x04003D61 RID: 15713
		public bool? disableSinglePawn;

		// Token: 0x04003D62 RID: 15714
		public Lord singlePawnLord;

		// Token: 0x04003D63 RID: 15715
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		// Token: 0x04003D64 RID: 15716
		public PawnGenerationRequest? singlePawnGenerationRequest;

		// Token: 0x04003D65 RID: 15717
		public Action<Thing> postThingSpawn;

		// Token: 0x04003D66 RID: 15718
		public Action<Thing> postThingGenerate;

		// Token: 0x04003D67 RID: 15719
		public int? mechanoidsCount;

		// Token: 0x04003D68 RID: 15720
		public int? hivesCount;

		// Token: 0x04003D69 RID: 15721
		public bool? disableHives;

		// Token: 0x04003D6A RID: 15722
		public Rot4? thingRot;

		// Token: 0x04003D6B RID: 15723
		public ThingDef wallStuff;

		// Token: 0x04003D6C RID: 15724
		public float? chanceToSkipWallBlock;

		// Token: 0x04003D6D RID: 15725
		public TerrainDef floorDef;

		// Token: 0x04003D6E RID: 15726
		public float? chanceToSkipFloor;

		// Token: 0x04003D6F RID: 15727
		public ThingDef filthDef;

		// Token: 0x04003D70 RID: 15728
		public FloatRange? filthDensity;

		// Token: 0x04003D71 RID: 15729
		public bool? floorOnlyIfTerrainSupports;

		// Token: 0x04003D72 RID: 15730
		public bool? allowBridgeOnAnyImpassableTerrain;

		// Token: 0x04003D73 RID: 15731
		public bool? clearEdificeOnly;

		// Token: 0x04003D74 RID: 15732
		public bool? clearFillageOnly;

		// Token: 0x04003D75 RID: 15733
		public bool? clearRoof;

		// Token: 0x04003D76 RID: 15734
		public int? ancientCryptosleepCasketGroupID;

		// Token: 0x04003D77 RID: 15735
		public PodContentsType? podContentsType;

		// Token: 0x04003D78 RID: 15736
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x04003D79 RID: 15737
		public ThingSetMakerParams? thingSetMakerParams;

		// Token: 0x04003D7A RID: 15738
		public IList<Thing> stockpileConcreteContents;

		// Token: 0x04003D7B RID: 15739
		public float? stockpileMarketValue;

		// Token: 0x04003D7C RID: 15740
		public int? innerStockpileSize;

		// Token: 0x04003D7D RID: 15741
		public int? edgeDefenseWidth;

		// Token: 0x04003D7E RID: 15742
		public int? edgeDefenseTurretsCount;

		// Token: 0x04003D7F RID: 15743
		public int? edgeDefenseMortarsCount;

		// Token: 0x04003D80 RID: 15744
		public int? edgeDefenseGuardsCount;

		// Token: 0x04003D81 RID: 15745
		public ThingDef mortarDef;

		// Token: 0x04003D82 RID: 15746
		public TerrainDef pathwayFloorDef;

		// Token: 0x04003D83 RID: 15747
		public ThingDef cultivatedPlantDef;

		// Token: 0x04003D84 RID: 15748
		public int? fillWithThingsPadding;

		// Token: 0x04003D85 RID: 15749
		public float? settlementPawnGroupPoints;

		// Token: 0x04003D86 RID: 15750
		public int? settlementPawnGroupSeed;

		// Token: 0x04003D87 RID: 15751
		public bool? streetHorizontal;

		// Token: 0x04003D88 RID: 15752
		public bool? edgeThingAvoidOtherEdgeThings;

		// Token: 0x04003D89 RID: 15753
		public bool? edgeThingMustReachMapEdge;

		// Token: 0x04003D8A RID: 15754
		public bool? allowPlacementOffEdge;

		// Token: 0x04003D8B RID: 15755
		public Rot4? thrustAxis;

		// Token: 0x04003D8C RID: 15756
		public FloatRange? hpPercentRange;

		// Token: 0x04003D8D RID: 15757
		public Thing conditionCauser;

		// Token: 0x04003D8E RID: 15758
		public bool? makeWarningLetter;

		// Token: 0x04003D8F RID: 15759
		public ThingFilter allowedMonumentThings;
	}
}
