using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	
	public struct ResolveParams
	{
		
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		
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

		
		public CellRect rect;

		
		public Faction faction;

		
		private Dictionary<string, object> custom;

		
		public PawnGroupMakerParms pawnGroupMakerParams;

		
		public PawnGroupKindDef pawnGroupKindDef;

		
		public RoofDef roofDef;

		
		public bool? noRoof;

		
		public bool? addRoomCenterToRootsToUnfog;

		
		public Thing singleThingToSpawn;

		
		public ThingDef singleThingDef;

		
		public ThingDef singleThingStuff;

		
		public int? singleThingStackCount;

		
		public bool? skipSingleThingIfHasToWipeBuildingOrDoesntFit;

		
		public bool? spawnBridgeIfTerrainCantSupportThing;

		
		public Pawn singlePawnToSpawn;

		
		public PawnKindDef singlePawnKindDef;

		
		public bool? disableSinglePawn;

		
		public Lord singlePawnLord;

		
		public Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;

		
		public PawnGenerationRequest? singlePawnGenerationRequest;

		
		public Action<Thing> postThingSpawn;

		
		public Action<Thing> postThingGenerate;

		
		public int? mechanoidsCount;

		
		public int? hivesCount;

		
		public bool? disableHives;

		
		public Rot4? thingRot;

		
		public ThingDef wallStuff;

		
		public float? chanceToSkipWallBlock;

		
		public TerrainDef floorDef;

		
		public float? chanceToSkipFloor;

		
		public ThingDef filthDef;

		
		public FloatRange? filthDensity;

		
		public bool? floorOnlyIfTerrainSupports;

		
		public bool? allowBridgeOnAnyImpassableTerrain;

		
		public bool? clearEdificeOnly;

		
		public bool? clearFillageOnly;

		
		public bool? clearRoof;

		
		public int? ancientCryptosleepCasketGroupID;

		
		public PodContentsType? podContentsType;

		
		public ThingSetMakerDef thingSetMakerDef;

		
		public ThingSetMakerParams? thingSetMakerParams;

		
		public IList<Thing> stockpileConcreteContents;

		
		public float? stockpileMarketValue;

		
		public int? innerStockpileSize;

		
		public int? edgeDefenseWidth;

		
		public int? edgeDefenseTurretsCount;

		
		public int? edgeDefenseMortarsCount;

		
		public int? edgeDefenseGuardsCount;

		
		public ThingDef mortarDef;

		
		public TerrainDef pathwayFloorDef;

		
		public ThingDef cultivatedPlantDef;

		
		public int? fillWithThingsPadding;

		
		public float? settlementPawnGroupPoints;

		
		public int? settlementPawnGroupSeed;

		
		public bool? streetHorizontal;

		
		public bool? edgeThingAvoidOtherEdgeThings;

		
		public bool? edgeThingMustReachMapEdge;

		
		public bool? allowPlacementOffEdge;

		
		public Rot4? thrustAxis;

		
		public FloatRange? hpPercentRange;

		
		public Thing conditionCauser;

		
		public bool? makeWarningLetter;

		
		public ThingFilter allowedMonumentThings;
	}
}
