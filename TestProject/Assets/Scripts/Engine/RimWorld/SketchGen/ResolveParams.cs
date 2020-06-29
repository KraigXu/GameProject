using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
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
			return string.Concat(new string[]
			{
				"sketch=",
				(this.sketch != null) ? this.sketch.ToString() : "null",
				", rect=",
				(this.rect != null) ? this.rect.ToString() : "null",
				", allowWood=",
				(this.allowWood != null) ? this.allowWood.ToString() : "null",
				", custom=",
				(this.custom != null) ? this.custom.Count.ToString() : "null",
				", symmetryOrigin=",
				(this.symmetryOrigin != null) ? this.symmetryOrigin.ToString() : "null",
				", symmetryVertical=",
				(this.symmetryVertical != null) ? this.symmetryVertical.ToString() : "null",
				", symmetryOriginIncluded=",
				(this.symmetryOriginIncluded != null) ? this.symmetryOriginIncluded.ToString() : "null",
				", symmetryClear=",
				(this.symmetryClear != null) ? this.symmetryClear.ToString() : "null",
				", connectedGroupsSameStuff=",
				(this.connectedGroupsSameStuff != null) ? this.connectedGroupsSameStuff.ToString() : "null",
				", assignRandomStuffTo=",
				(this.assignRandomStuffTo != null) ? this.assignRandomStuffTo.ToString() : "null",
				", cornerThing=",
				(this.cornerThing != null) ? this.cornerThing.ToString() : "null",
				", floorFillRoomsOnly=",
				(this.floorFillRoomsOnly != null) ? this.floorFillRoomsOnly.ToString() : "null",
				", singleFloorType=",
				(this.singleFloorType != null) ? this.singleFloorType.ToString() : "null",
				", onlyStoneFloors=",
				(this.onlyStoneFloors != null) ? this.onlyStoneFloors.ToString() : "null",
				", thingCentral=",
				(this.thingCentral != null) ? this.thingCentral.ToString() : "null",
				", wallEdgeThing=",
				(this.wallEdgeThing != null) ? this.wallEdgeThing.ToString() : "null",
				", monumentSize=",
				(this.monumentSize != null) ? this.monumentSize.ToString() : "null",
				", monumentOpen=",
				(this.monumentOpen != null) ? this.monumentOpen.ToString() : "null",
				", allowMonumentDoors=",
				(this.allowMonumentDoors != null) ? this.allowMonumentDoors.ToString() : "null",
				", allowedMonumentThings=",
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null",
				", useOnlyStonesAvailableOnMap=",
				(this.useOnlyStonesAvailableOnMap != null) ? this.useOnlyStonesAvailableOnMap.ToString() : "null",
				", allowConcrete=",
				(this.allowConcrete != null) ? this.allowConcrete.ToString() : "null",
				", allowFlammableWalls=",
				(this.allowFlammableWalls != null) ? this.allowFlammableWalls.ToString() : "null",
				", onlyBuildableByPlayer=",
				(this.onlyBuildableByPlayer != null) ? this.onlyBuildableByPlayer.ToString() : "null",
				", addFloor=",
				(this.addFloors != null) ? this.addFloors.ToString() : "null",
				", requireFloor=",
				(this.requireFloor != null) ? this.requireFloor.ToString() : "null",
				", mechClusterSize=",
				(this.mechClusterSize != null) ? this.mechClusterSize.ToString() : "null",
				", mechClusterDormant=",
				(this.mechClusterDormant != null) ? this.mechClusterDormant.ToString() : "null",
				", mechClusterForMap=",
				(this.mechClusterForMap != null) ? this.mechClusterForMap.ToString() : "null"
			});
		}

		
		public Sketch sketch;

		
		public CellRect? rect;

		
		public bool? allowWood;

		
		public float? points;

		
		public float? totalPoints;

		
		public int? symmetryOrigin;

		
		public bool? symmetryVertical;

		
		public bool? symmetryOriginIncluded;

		
		public bool? symmetryClear;

		
		public bool? connectedGroupsSameStuff;

		
		public ThingDef assignRandomStuffTo;

		
		public ThingDef cornerThing;

		
		public bool? floorFillRoomsOnly;

		
		public bool? singleFloorType;

		
		public bool? onlyStoneFloors;

		
		public ThingDef thingCentral;

		
		public ThingDef wallEdgeThing;

		
		public IntVec2? monumentSize;

		
		public bool? monumentOpen;

		
		public bool? allowMonumentDoors;

		
		public ThingFilter allowedMonumentThings;

		
		public Map useOnlyStonesAvailableOnMap;

		
		public bool? allowConcrete;

		
		public bool? allowFlammableWalls;

		
		public bool? onlyBuildableByPlayer;

		
		public bool? addFloors;

		
		public bool? requireFloor;

		
		public IntVec2? mechClusterSize;

		
		public bool? mechClusterDormant;

		
		public Map mechClusterForMap;

		
		private Dictionary<string, object> custom;
	}
}
