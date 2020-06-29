﻿using System;
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

		
		
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}
	}
}
