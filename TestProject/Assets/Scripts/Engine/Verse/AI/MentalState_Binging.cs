using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200054F RID: 1359
	public class MentalState_Binging : MentalState
	{
		// Token: 0x060026DA RID: 9946 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
