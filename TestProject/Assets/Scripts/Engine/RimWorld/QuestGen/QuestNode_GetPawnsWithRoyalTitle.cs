﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				IEnumerable<Pawn> filteredPawns = this.GetFilteredPawns(this.pawns.GetValue(slate));
				slate.Set<IEnumerable<Pawn>>(this.storeAs.GetValue(slate), filteredPawns, false);
				if (this.storeCountAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeCountAs.GetValue(slate), filteredPawns.Count<Pawn>(), false);
				}
				if (this.storePawnsLabelAs.GetValue(slate) != null)
				{
					slate.Set<string>(this.storePawnsLabelAs.GetValue(slate), (from p in filteredPawns
					select p.LabelNoCountColored.Resolve()).ToCommaList(true), false);
				}
			}
		}

		
		private IEnumerable<Pawn> GetFilteredPawns(List<Pawn> pawns)
		{
			Slate slate = QuestGen.slate;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].royalty != null && pawns[i].royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		
		public SlateRef<List<Pawn>> pawns;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		
		[NoTranslate]
		public SlateRef<string> storePawnsLabelAs;
	}
}
