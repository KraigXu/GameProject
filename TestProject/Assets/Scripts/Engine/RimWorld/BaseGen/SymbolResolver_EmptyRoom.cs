using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_EmptyRoom : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, false);
			if (rp.noRoof == null || !rp.noRoof.Value)
			{
				BaseGenCore.symbolStack.Push("roof", rp, null);
			}
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = thingDef;
			BaseGenCore.symbolStack.Push("edgeWalls", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = floorDef;
			BaseGenCore.symbolStack.Push("floor", resolveParams2, null);
			BaseGenCore.symbolStack.Push("clear", rp, null);
			if (rp.addRoomCenterToRootsToUnfog != null && rp.addRoomCenterToRootsToUnfog.Value && Current.ProgramState == ProgramState.MapInitializing)
			{
				MapGenerator.rootsToUnfog.Add(rp.rect.CenterCell);
			}
		}
	}
}
