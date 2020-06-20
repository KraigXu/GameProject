using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020010E5 RID: 4325
	public abstract class SymbolResolver
	{
		// Token: 0x060065B0 RID: 26032 RVA: 0x0023A2B1 File Offset: 0x002384B1
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x060065B1 RID: 26033
		public abstract void Resolve(ResolveParams rp);

		// Token: 0x04003DDB RID: 15835
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04003DDC RID: 15836
		public float selectionWeight = 1f;
	}
}
