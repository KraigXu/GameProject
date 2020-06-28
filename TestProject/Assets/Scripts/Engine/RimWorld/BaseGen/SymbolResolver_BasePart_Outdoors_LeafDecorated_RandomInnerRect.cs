using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010A1 RID: 4257
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x060064DD RID: 25821 RVA: 0x00232668 File Offset: 0x00230868
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x002326C0 File Offset: 0x002308C0
		public override void Resolve(ResolveParams rp)
		{
			int num = Rand.RangeInclusive(5, rp.rect.Width - 1);
			int num2 = Rand.RangeInclusive(5, rp.rect.Height - 1);
			int num3 = Rand.RangeInclusive(0, rp.rect.Width - num);
			int num4 = Rand.RangeInclusive(0, rp.rect.Height - num2);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX + num3, rp.rect.minZ + num4, num, num2);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams, null);
		}

		// Token: 0x04003DA1 RID: 15777
		private const int MinLength = 5;

		// Token: 0x04003DA2 RID: 15778
		private const int MaxRectSize = 15;
	}
}
