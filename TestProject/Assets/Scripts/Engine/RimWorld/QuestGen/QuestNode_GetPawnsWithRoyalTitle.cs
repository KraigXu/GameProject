using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001136 RID: 4406
	public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
	{
		// Token: 0x060066F8 RID: 26360 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060066F9 RID: 26361 RVA: 0x002410FC File Offset: 0x0023F2FC
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

		// Token: 0x060066FA RID: 26362 RVA: 0x002411BA File Offset: 0x0023F3BA
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

		// Token: 0x04003F20 RID: 16160
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x04003F21 RID: 16161
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F22 RID: 16162
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x04003F23 RID: 16163
		[NoTranslate]
		public SlateRef<string> storePawnsLabelAs;
	}
}
