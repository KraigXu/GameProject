using System;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_EdgeStreet : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, 1);
			resolveParams.floorDef = floorDef;
			resolveParams.streetHorizontal = new bool?(true);
			BaseGenCore.symbolStack.Push("street", resolveParams, null);
			if (rp.rect.Height > 1)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.maxZ, rp.rect.Width, 1);
				resolveParams2.floorDef = floorDef;
				resolveParams2.streetHorizontal = new bool?(true);
				BaseGenCore.symbolStack.Push("street", resolveParams2, null);
			}
			ResolveParams resolveParams3 = rp;
			resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ, 1, rp.rect.Height);
			resolveParams3.floorDef = floorDef;
			resolveParams3.streetHorizontal = new bool?(false);
			BaseGenCore.symbolStack.Push("street", resolveParams3, null);
			if (rp.rect.Width > 1)
			{
				ResolveParams resolveParams4 = rp;
				resolveParams4.rect = new CellRect(rp.rect.maxX, rp.rect.minZ, 1, rp.rect.Height);
				resolveParams4.floorDef = floorDef;
				resolveParams4.streetHorizontal = new bool?(false);
				BaseGenCore.symbolStack.Push("street", resolveParams4, null);
			}
		}
	}
}
