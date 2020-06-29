using System;

namespace RimWorld
{
	
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		
		// (get) Token: 0x06003B6C RID: 15212 RVA: 0x0013A1CF File Offset: 0x001383CF
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}
	}
}
