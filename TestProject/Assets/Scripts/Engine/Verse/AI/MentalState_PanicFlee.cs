using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000551 RID: 1361
	public class MentalState_PanicFlee : MentalState
	{
		// Token: 0x060026E5 RID: 9957 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
