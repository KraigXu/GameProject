using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDD RID: 3805
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_MentalState : PawnColumnWorker_Icon
	{
		// Token: 0x06005D42 RID: 23874 RVA: 0x002048E4 File Offset: 0x00202AE4
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (!pawn.InMentalState)
			{
				return null;
			}
			if (!pawn.InAggroMentalState)
			{
				return PawnColumnWorker_MentalState.IconNonAggro;
			}
			return PawnColumnWorker_MentalState.IconAggro;
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x00204903 File Offset: 0x00202B03
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.InMentalState ? "IsInMentalState".Translate(pawn.MentalState.def.LabelCap) : null;
		}

		// Token: 0x040032C0 RID: 12992
		private static readonly Texture2D IconNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x040032C1 RID: 12993
		private static readonly Texture2D IconAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);
	}
}
