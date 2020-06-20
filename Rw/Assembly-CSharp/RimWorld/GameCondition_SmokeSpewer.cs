using System;

namespace RimWorld
{
	// Token: 0x020009BB RID: 2491
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		// Token: 0x17000AB3 RID: 2739
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
