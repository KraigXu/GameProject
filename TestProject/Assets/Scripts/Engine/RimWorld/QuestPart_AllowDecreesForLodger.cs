using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000967 RID: 2407
	public class QuestPart_AllowDecreesForLodger : QuestPart
	{
		// Token: 0x0600390A RID: 14602 RVA: 0x0012FFAF File Offset: 0x0012E1AF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.lodger, "lodger", false);
		}

		// Token: 0x040021A2 RID: 8610
		public Pawn lodger;
	}
}
