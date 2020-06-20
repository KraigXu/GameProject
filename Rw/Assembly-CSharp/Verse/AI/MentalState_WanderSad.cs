using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000556 RID: 1366
	public class MentalState_WanderSad : MentalState
	{
		// Token: 0x060026F4 RID: 9972 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
