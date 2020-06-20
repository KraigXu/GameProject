using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001082 RID: 4226
	public struct ResolveParams
	{
		// Token: 0x06006454 RID: 25684 RVA: 0x0022BDBF File Offset: 0x00229FBF
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x0022BDCF File Offset: 0x00229FCF
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x0022BDDD File Offset: 0x00229FDD
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x0022BDEC File Offset: 0x00229FEC
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x0022BDFC File Offset: 0x00229FFC
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

		// Token: 0x04003CFF RID: 15615
		public Sketch sketch;

		// Token: 0x04003D00 RID: 15616
		public CellRect? rect;

		// Token: 0x04003D01 RID: 15617
		public bool? allowWood;

		// Token: 0x04003D02 RID: 15618
		public float? points;

		// Token: 0x04003D03 RID: 15619
		public float? totalPoints;

		// Token: 0x04003D04 RID: 15620
		public int? symmetryOrigin;

		// Token: 0x04003D05 RID: 15621
		public bool? symmetryVertical;

		// Token: 0x04003D06 RID: 15622
		public bool? symmetryOriginIncluded;

		// Token: 0x04003D07 RID: 15623
		public bool? symmetryClear;

		// Token: 0x04003D08 RID: 15624
		public bool? connectedGroupsSameStuff;

		// Token: 0x04003D09 RID: 15625
		public ThingDef assignRandomStuffTo;

		// Token: 0x04003D0A RID: 15626
		public ThingDef cornerThing;

		// Token: 0x04003D0B RID: 15627
		public bool? floorFillRoomsOnly;

		// Token: 0x04003D0C RID: 15628
		public bool? singleFloorType;

		// Token: 0x04003D0D RID: 15629
		public bool? onlyStoneFloors;

		// Token: 0x04003D0E RID: 15630
		public ThingDef thingCentral;

		// Token: 0x04003D0F RID: 15631
		public ThingDef wallEdgeThing;

		// Token: 0x04003D10 RID: 15632
		public IntVec2? monumentSize;

		// Token: 0x04003D11 RID: 15633
		public bool? monumentOpen;

		// Token: 0x04003D12 RID: 15634
		public bool? allowMonumentDoors;

		// Token: 0x04003D13 RID: 15635
		public ThingFilter allowedMonumentThings;

		// Token: 0x04003D14 RID: 15636
		public Map useOnlyStonesAvailableOnMap;

		// Token: 0x04003D15 RID: 15637
		public bool? allowConcrete;

		// Token: 0x04003D16 RID: 15638
		public bool? allowFlammableWalls;

		// Token: 0x04003D17 RID: 15639
		public bool? onlyBuildableByPlayer;

		// Token: 0x04003D18 RID: 15640
		public bool? addFloors;

		// Token: 0x04003D19 RID: 15641
		public bool? requireFloor;

		// Token: 0x04003D1A RID: 15642
		public IntVec2? mechClusterSize;

		// Token: 0x04003D1B RID: 15643
		public bool? mechClusterDormant;

		// Token: 0x04003D1C RID: 15644
		public Map mechClusterForMap;

		// Token: 0x04003D1D RID: 15645
		private Dictionary<string, object> custom;
	}
}
