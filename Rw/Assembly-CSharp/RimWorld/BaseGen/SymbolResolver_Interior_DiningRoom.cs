using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010DE RID: 4318
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x060065A1 RID: 26017 RVA: 0x0023926C File Offset: 0x0023746C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("randomlyPlaceMealsOnTables", rp, null);
			BaseGen.symbolStack.Push("placeChairsNearTables", rp, null);
			int num = Mathf.Max(GenMath.RoundRandom((float)rp.rect.Area / 20f), 1);
			for (int i = 0; i < num; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Table2x2c;
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
		}
	}
}
