using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200109D RID: 4253
	public class SymbolResolver_BasePart_Indoors : SymbolResolver
	{
		// Token: 0x060064CA RID: 25802 RVA: 0x002319A4 File Offset: 0x0022FBA4
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 13 || rp.rect.Height > 13 || ((rp.rect.Width >= 9 || rp.rect.Height >= 9) && Rand.Chance(0.3f));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_indoors_division", rp, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_indoors_leaf", rp, null);
		}
	}
}
