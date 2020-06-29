using System;
using Verse;

namespace RimWorld
{
	
	public class LordToil_Concert : LordToil_Party
	{
		
		
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		
		public LordToil_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef, joyPerTick)
		{
			this.organizer = organizer;
		}

		
		private Pawn organizer;
	}
}
