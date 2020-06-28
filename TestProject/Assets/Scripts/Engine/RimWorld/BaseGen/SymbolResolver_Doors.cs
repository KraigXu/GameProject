using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010AC RID: 4268
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x060064FF RID: 25855 RVA: 0x00233350 File Offset: 0x00231550
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp, null);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
		}

		// Token: 0x04003DA9 RID: 15785
		private const float ExtraDoorChance = 0.25f;
	}
}
