using System;

namespace Verse
{
	
	public class HediffGiver_Event : HediffGiver
	{
		
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		
		private float chance = 1f;
	}
}
