using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C75 RID: 3189
	public interface ICommunicable
	{
		// Token: 0x06004C7F RID: 19583
		string GetCallLabel();

		// Token: 0x06004C80 RID: 19584
		string GetInfoText();

		// Token: 0x06004C81 RID: 19585
		void TryOpenComms(Pawn negotiator);

		// Token: 0x06004C82 RID: 19586
		Faction GetFaction();

		// Token: 0x06004C83 RID: 19587
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
