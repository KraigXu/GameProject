using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000600 RID: 1536
	public class Trigger_OnHumanlikeHarmAnyThing : Trigger
	{
		// Token: 0x06002A22 RID: 10786 RVA: 0x000F63D2 File Offset: 0x000F45D2
		public Trigger_OnHumanlikeHarmAnyThing(List<Thing> things)
		{
			this.things = things;
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000F63E4 File Offset: 0x000F45E4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			Pawn pawn;
			return signal.dinfo.Def != null && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.dinfo.Instigator != null && (pawn = (signal.dinfo.Instigator as Pawn)) != null && pawn.RaceProps.Humanlike && this.things.Contains(signal.thing);
		}

		// Token: 0x0400192D RID: 6445
		private List<Thing> things;
	}
}
