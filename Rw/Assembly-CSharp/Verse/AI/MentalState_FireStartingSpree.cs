using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000558 RID: 1368
	public class MentalState_FireStartingSpree : MentalState
	{
		// Token: 0x060026F8 RID: 9976 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
