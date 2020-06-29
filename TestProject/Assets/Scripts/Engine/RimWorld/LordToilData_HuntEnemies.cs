using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToilData_HuntEnemies : LordToilData
	{
		
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}
