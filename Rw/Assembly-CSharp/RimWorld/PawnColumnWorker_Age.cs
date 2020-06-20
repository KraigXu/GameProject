using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE2 RID: 3810
	public class PawnColumnWorker_Age : PawnColumnWorker_Text
	{
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x06005D5C RID: 23900 RVA: 0x00010306 File Offset: 0x0000E506
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x00204A7C File Offset: 0x00202C7C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalYears.CompareTo(b.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x00204AA8 File Offset: 0x00202CA8
		protected override string GetTextFor(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears.ToString();
		}
	}
}
