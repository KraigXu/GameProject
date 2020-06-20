using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000557 RID: 1367
	public class MentalState_WanderPsychotic : MentalState
	{
		// Token: 0x060026F6 RID: 9974 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
