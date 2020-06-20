using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000552 RID: 1362
	public class MentalState_GiveUpExit : MentalState
	{
		// Token: 0x060026E7 RID: 9959 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
