using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		
		public override void Resolve(ResolveParams rp)
		{
			BaseGenCore.symbolStack.Push("indoorLighting", rp, null);
			BaseGenCore.symbolStack.Push("randomlyPlaceMealsOnTables", rp, null);
			BaseGenCore.symbolStack.Push("placeChairsNearTables", rp, null);
			int num = Mathf.Max(GenMath.RoundRandom((float)rp.rect.Area / 20f), 1);
			for (int i = 0; i < num; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Table2x2c;
				BaseGenCore.symbolStack.Push("thing", resolveParams, null);
			}
		}
	}
}
