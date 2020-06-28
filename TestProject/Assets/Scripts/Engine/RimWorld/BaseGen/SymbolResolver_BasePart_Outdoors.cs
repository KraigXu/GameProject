using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A7 RID: 4263
	public class SymbolResolver_BasePart_Outdoors : SymbolResolver
	{
		// Token: 0x060064F0 RID: 25840 RVA: 0x00232C40 File Offset: 0x00230E40
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 23 || rp.rect.Height > 23 || ((rp.rect.Width >= 11 || rp.rect.Height >= 11) && Rand.Bool);
			ResolveParams resolveParams = rp;
			resolveParams.pathwayFloorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_outdoors_division", resolveParams, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leafPossiblyDecorated", resolveParams, null);
		}
	}
}
