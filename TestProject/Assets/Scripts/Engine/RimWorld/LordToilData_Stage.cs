using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToilData_Stage : LordToilData
	{
		
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		
		public IntVec3 stagingPoint;
	}
}
