using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000854 RID: 2132
	public class ThoughtWorker_QuestPart : ThoughtWorker
	{
		// Token: 0x060034C6 RID: 13510 RVA: 0x00120D9C File Offset: 0x0011EF9C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			QuestPart_SituationalThought questPart_SituationalThought = this.FindQuestPart(p);
			if (questPart_SituationalThought == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(questPart_SituationalThought.stage);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x00120DC8 File Offset: 0x0011EFC8
		public QuestPart_SituationalThought FindQuestPart(Pawn p)
		{
			List<QuestPart_SituationalThought> situationalThoughtQuestParts = Find.QuestManager.SituationalThoughtQuestParts;
			for (int i = 0; i < situationalThoughtQuestParts.Count; i++)
			{
				if (situationalThoughtQuestParts[i].quest.State == QuestState.Ongoing && situationalThoughtQuestParts[i].State == QuestPartState.Enabled && situationalThoughtQuestParts[i].def == this.def && situationalThoughtQuestParts[i].pawn == p)
				{
					return situationalThoughtQuestParts[i];
				}
			}
			return null;
		}
	}
}
