using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200054E RID: 1358
	public class MentalState_Berserk : MentalState
	{
		// Token: 0x060026D6 RID: 9942 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHostileTo(Thing t)
		{
			return true;
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
