using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B42 RID: 2882
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x060043C8 RID: 17352 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
