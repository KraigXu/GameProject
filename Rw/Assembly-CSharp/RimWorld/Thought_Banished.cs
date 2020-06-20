using System;

namespace RimWorld
{
	// Token: 0x0200080A RID: 2058
	public class Thought_Banished : Thought_Memory
	{
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x0011ED08 File Offset: 0x0011CF08
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.otherPawn.Faction == this.pawn.Faction;
			}
		}
	}
}
