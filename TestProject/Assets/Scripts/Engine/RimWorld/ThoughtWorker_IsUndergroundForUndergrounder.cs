using System;
using Verse;

namespace RimWorld
{
	
	public class ThoughtWorker_IsUndergroundForUndergrounder : ThoughtWorker
	{
		
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && flag;
		}
	}
}
