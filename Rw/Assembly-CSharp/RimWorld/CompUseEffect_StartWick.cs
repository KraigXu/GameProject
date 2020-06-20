using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA7 RID: 3495
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x060054DF RID: 21727 RVA: 0x001C4876 File Offset: 0x001C2A76
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
