using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JoyGiver_Meditate : JoyGiver_InPrivateRoom
	{
		
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return base.TryGiveJobWhileInBed(pawn);
			}
			return MeditationUtility.GetMeditationJob(pawn, true);
		}

		
		public override Job TryGiveJob(Pawn pawn)
		{
			if (ModsConfig.RoyaltyActive)
			{
				return MeditationUtility.GetMeditationJob(pawn, true);
			}
			return base.TryGiveJob(pawn);
		}
	}
}
