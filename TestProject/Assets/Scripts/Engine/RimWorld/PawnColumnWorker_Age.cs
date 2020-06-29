using System;
using Verse;

namespace RimWorld
{
	
	public class PawnColumnWorker_Age : PawnColumnWorker_Text
	{
		
		// (get) Token: 0x06005D5C RID: 23900 RVA: 0x00010306 File Offset: 0x0000E506
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalYears.CompareTo(b.ageTracker.AgeBiologicalYears);
		}

		
		protected override string GetTextFor(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears.ToString();
		}
	}
}
