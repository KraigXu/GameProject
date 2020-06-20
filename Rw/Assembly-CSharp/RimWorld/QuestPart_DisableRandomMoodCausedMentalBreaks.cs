using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000972 RID: 2418
	public class QuestPart_DisableRandomMoodCausedMentalBreaks : QuestPartActivable
	{
		// Token: 0x0600394A RID: 14666 RVA: 0x00130FB4 File Offset: 0x0012F1B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x00131010 File Offset: 0x0012F210
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040021C5 RID: 8645
		public List<Pawn> pawns = new List<Pawn>();
	}
}
