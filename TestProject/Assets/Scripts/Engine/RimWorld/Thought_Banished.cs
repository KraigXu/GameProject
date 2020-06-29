using System;

namespace RimWorld
{
	
	public class Thought_Banished : Thought_Memory
	{
		
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
