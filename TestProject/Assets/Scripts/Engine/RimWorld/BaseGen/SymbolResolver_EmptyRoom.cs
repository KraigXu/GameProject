using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010B1 RID: 4273
	public class SymbolResolver_EmptyRoom : SymbolResolver
	{
		// Token: 0x0600650F RID: 25871 RVA: 0x00233DAC File Offset: 0x00231FAC
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, false);
			if (rp.noRoof == null || !rp.noRoof.Value)
			{
				BaseGen.symbolStack.Push("roof", rp, null);
			}
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = thingDef;
			BaseGen.symbolStack.Push("edgeWalls", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams2, null);
			BaseGen.symbolStack.Push("clear", rp, null);
			if (rp.addRoomCenterToRootsToUnfog != null && rp.addRoomCenterToRootsToUnfog.Value && Current.ProgramState == ProgramState.MapInitializing)
			{
				MapGenerator.rootsToUnfog.Add(rp.rect.CenterCell);
			}
		}
	}
}
