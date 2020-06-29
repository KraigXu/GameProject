using System;
using Verse;

namespace RimWorld
{
	
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		
		// (get) Token: 0x060054B6 RID: 21686 RVA: 0x001C3A91 File Offset: 0x001C1C91
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
