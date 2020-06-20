using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000844 RID: 2116
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x06003498 RID: 13464 RVA: 0x00120428 File Offset: 0x0011E628
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.equipment.Primary == null)
			{
				return false;
			}
			List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsIncendiary())
				{
					return true;
				}
			}
			return false;
		}
	}
}
