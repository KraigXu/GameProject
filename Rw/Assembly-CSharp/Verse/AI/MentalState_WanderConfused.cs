using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000555 RID: 1365
	public class MentalState_WanderConfused : MentalState
	{
		// Token: 0x060026F2 RID: 9970 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
