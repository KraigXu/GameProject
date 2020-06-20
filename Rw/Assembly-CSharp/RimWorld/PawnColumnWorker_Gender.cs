using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED9 RID: 3801
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x06005D29 RID: 23849 RVA: 0x002046E6 File Offset: 0x002028E6
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x002046F3 File Offset: 0x002028F3
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.GetGenderLabel().CapitalizeFirst();
		}
	}
}
