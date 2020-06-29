using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetRelationsInfo : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
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

		
		public SlateRef<Pawn> pawn;

		
		public SlateRef<IEnumerable<Pawn>> otherPawns;

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<string> nonRelatedLabel;

		
		public SlateRef<string> nonRelatedLabelPlural;

		
		private static List<string> tmpRelations = new List<string>();
	}
}
