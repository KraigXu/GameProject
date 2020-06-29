using System;

namespace RimWorld
{
	
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		
		
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}
	}
}
