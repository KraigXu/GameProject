using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public abstract class LordJob_VoluntarilyJoinable : LordJob
	{
		
		public virtual float VoluntaryJoinPriorityFor(Pawn p)
		{
			return 0f;
		}

		
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
