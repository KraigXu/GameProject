using System;

namespace RimWorld
{
	
	public class Thought_Banished : Thought_Memory
	{
		
		
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.otherPawn.Faction == this.pawn.Faction;
			}
		}
	}
}
