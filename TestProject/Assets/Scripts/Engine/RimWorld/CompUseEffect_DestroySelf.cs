using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9E RID: 3486
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x060054B6 RID: 21686 RVA: 0x001C3A91 File Offset: 0x001C1C91
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x001C3A98 File Offset: 0x001C1C98
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
