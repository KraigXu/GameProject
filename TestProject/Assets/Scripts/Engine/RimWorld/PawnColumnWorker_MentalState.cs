using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_MentalState : PawnColumnWorker_Icon
	{
		
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

		
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.InMentalState ? "IsInMentalState".Translate(pawn.MentalState.def.LabelCap) : null;
		}

		
		private static readonly Texture2D IconNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		
		private static readonly Texture2D IconAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);
	}
}
