using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_DisableRandomMoodCausedMentalBreaks : QuestPartActivable
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public List<Pawn> pawns = new List<Pawn>();
	}
}
