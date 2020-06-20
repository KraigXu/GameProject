using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010D0 RID: 4304
	public class SymbolResolver_OutdoorLighting : SymbolResolver
	{
		// Token: 0x06006576 RID: 25974 RVA: 0x00237A78 File Offset: 0x00235C78
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			this.FindNearbyGlowers(rp.rect);
			for (int i = 0; i < rp.rect.Area / 4; i++)
			{
				IntVec3 randomCell = rp.rect.RandomCell;
				if (randomCell.Standable(map) && randomCell.GetFirstItem(map) == null && randomCell.GetFirstPawn(map) == null && randomCell.GetFirstBuilding(map) == null)
				{
					Region region = randomCell.GetRegion(map, RegionType.Set_Passable);
					if (region != null && region.Room.PsychologicallyOutdoors && region.Room.UsesOutdoorTemperature && !this.AnyGlowerNearby(randomCell) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(randomCell, map))
					{
						if (rp.spawnBridgeIfTerrainCantSupportThing == null || rp.spawnBridgeIfTerrainCantSupportThing.Value)
						{
							BaseGenUtility.CheckSpawnBridgeUnder(thingDef, randomCell, Rot4.North);
						}
						Thing thing = GenSpawn.Spawn(thingDef, randomCell, map, WipeMode.Vanish);
						if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
						{
							thing.SetFaction(rp.faction, null);
						}
						SymbolResolver_OutdoorLighting.nearbyGlowers.Add(thing.TryGetComp<CompGlower>());
					}
				}
			}
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x00237BE0 File Offset: 0x00235DE0
		private void FindNearbyGlowers(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
			rect = rect.ExpandedBy(4);
			rect = rect.ClipInsideMap(map);
			foreach (IntVec3 intVec in rect)
			{
				Region region = intVec.GetRegion(map, RegionType.Set_Passable);
				if (region != null && region.Room.PsychologicallyOutdoors)
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						CompGlower compGlower = thingList[i].TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							SymbolResolver_OutdoorLighting.nearbyGlowers.Add(compGlower);
						}
					}
				}
			}
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x00237CAC File Offset: 0x00235EAC
		private bool AnyGlowerNearby(IntVec3 c)
		{
			for (int i = 0; i < SymbolResolver_OutdoorLighting.nearbyGlowers.Count; i++)
			{
				if (c.InHorDistOf(SymbolResolver_OutdoorLighting.nearbyGlowers[i].parent.Position, SymbolResolver_OutdoorLighting.nearbyGlowers[i].Props.glowRadius + 2f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003DC7 RID: 15815
		private static List<CompGlower> nearbyGlowers = new List<CompGlower>();

		// Token: 0x04003DC8 RID: 15816
		private const float Margin = 2f;
	}
}
