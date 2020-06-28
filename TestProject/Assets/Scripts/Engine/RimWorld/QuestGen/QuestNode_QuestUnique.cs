using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001182 RID: 4482
	public class QuestNode_QuestUnique : QuestNode
	{
		// Token: 0x06006807 RID: 26631 RVA: 0x002460AB File Offset: 0x002442AB
		public static string GetProcessedTag(string tag, Faction faction)
		{
			if (faction == null)
			{
				return tag;
			}
			return tag + "_" + faction.Name;
		}

		// Token: 0x06006808 RID: 26632 RVA: 0x002460C3 File Offset: 0x002442C3
		private string GetProcessedTag(Slate slate)
		{
			return QuestNode_QuestUnique.GetProcessedTag(this.tag.GetValue(slate), this.faction.GetValue(slate));
		}

		// Token: 0x06006809 RID: 26633 RVA: 0x002460E4 File Offset: 0x002442E4
		protected override void RunInt()
		{
			string processedTag = this.GetProcessedTag(QuestGen.slate);
			QuestUtility.AddQuestTag(ref QuestGen.quest.tags, processedTag);
			if (this.storeProcessedTagAs.GetValue(QuestGen.slate) != null)
			{
				QuestGen.slate.Set<string>(this.storeProcessedTagAs.GetValue(QuestGen.slate), processedTag, false);
			}
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x0024613C File Offset: 0x0024433C
		protected override bool TestRunInt(Slate slate)
		{
			string processedTag = this.GetProcessedTag(slate);
			if (this.storeProcessedTagAs.GetValue(slate) != null)
			{
				slate.Set<string>(this.storeProcessedTagAs.GetValue(slate), processedTag, false);
			}
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (quest.State == QuestState.Ongoing && quest.tags.Contains(processedTag))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400405A RID: 16474
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400405B RID: 16475
		public SlateRef<Faction> faction;

		// Token: 0x0400405C RID: 16476
		[NoTranslate]
		public SlateRef<string> storeProcessedTagAs;
	}
}
