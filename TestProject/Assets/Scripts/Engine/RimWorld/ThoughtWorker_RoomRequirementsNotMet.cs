using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200084B RID: 2123
	public abstract class ThoughtWorker_RoomRequirementsNotMet : ThoughtWorker
	{
		// Token: 0x060034AE RID: 13486
		protected abstract IEnumerable<string> UnmetRequirements(Pawn p);

		// Token: 0x060034AF RID: 13487 RVA: 0x00120B17 File Offset: 0x0011ED17
		protected bool Active(Pawn p)
		{
			return p.royalty != null && p.MapHeld != null && p.MapHeld.IsPlayerHome && this.UnmetRequirements(p).Any<string>();
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00120B44 File Offset: 0x0011ED44
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
