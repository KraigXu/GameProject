using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5A RID: 3162
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x06004B8E RID: 19342 RVA: 0x001972A4 File Offset: 0x001954A4
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
