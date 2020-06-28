using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001141 RID: 4417
	public class QuestNode_GetRelationsInfo : QuestNode
	{
		// Token: 0x06006724 RID: 26404 RVA: 0x00241B35 File Offset: 0x0023FD35
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06006725 RID: 26405 RVA: 0x00241B3F File Offset: 0x0023FD3F
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06006726 RID: 26406 RVA: 0x00241B4C File Offset: 0x0023FD4C
		private void SetVars(Slate slate)
		{
			if (this.pawn.GetValue(slate) == null)
			{
				return;
			}
			if (this.otherPawns.GetValue(slate) == null)
			{
				return;
			}
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
			int num = 0;
			foreach (Pawn other in this.otherPawns.GetValue(slate))
			{
				PawnRelationDef mostImportantRelation = this.pawn.GetValue(slate).GetMostImportantRelation(other);
				if (mostImportantRelation != null)
				{
					QuestNode_GetRelationsInfo.tmpRelations.Add(mostImportantRelation.GetGenderSpecificLabel(other));
				}
				else
				{
					num++;
				}
			}
			if (num == 1)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(this.nonRelatedLabel.GetValue(slate));
			}
			else if (num >= 2)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(num + " " + this.nonRelatedLabelPlural.GetValue(slate));
			}
			if (!QuestNode_GetRelationsInfo.tmpRelations.Any<string>())
			{
				return;
			}
			slate.Set<string>(this.storeAs.GetValue(slate), QuestNode_GetRelationsInfo.tmpRelations.ToCommaList(true), false);
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
		}

		// Token: 0x04003F46 RID: 16198
		public SlateRef<Pawn> pawn;

		// Token: 0x04003F47 RID: 16199
		public SlateRef<IEnumerable<Pawn>> otherPawns;

		// Token: 0x04003F48 RID: 16200
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F49 RID: 16201
		public SlateRef<string> nonRelatedLabel;

		// Token: 0x04003F4A RID: 16202
		public SlateRef<string> nonRelatedLabelPlural;

		// Token: 0x04003F4B RID: 16203
		private static List<string> tmpRelations = new List<string>();
	}
}
