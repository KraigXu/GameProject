﻿using System;
using Verse;

namespace RimWorld
{
	
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		
		
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		
		protected override Pawn GetFollowee(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return null;
			}
			return pawn.playerSettings.Master;
		}

		
		protected override float GetRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 3f;
		}

		
		public const float RadiusUnreleased = 3f;

		
		public const float RadiusReleased = 50f;
	}
}
