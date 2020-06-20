using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000843 RID: 2115
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06003496 RID: 13462 RVA: 0x001203F9 File Offset: 0x0011E5F9
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
