using System;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_QuestUnique : QuestNode
	{
		
		public static string GetProcessedTag(string tag, Faction faction)
		{
			if (faction == null)
			{
				return tag;
			}
			return tag + "_" + faction.Name;
		}

		
		private string GetProcessedTag(Slate slate)
		{
			return QuestNode_QuestUnique.GetProcessedTag(this.tag.GetValue(slate), this.faction.GetValue(slate));
		}

		
		protected override void RunInt()
		{
			string processedTag = this.GetProcessedTag(QuestGen.slate);
			QuestUtility.AddQuestTag(ref QuestGen.quest.tags, processedTag);
			if (this.storeProcessedTagAs.GetValue(QuestGen.slate) != null)
			{
				QuestGen.slate.Set<string>(this.storeProcessedTagAs.GetValue(QuestGen.slate), processedTag, false);
			}
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> tag;

		
		public SlateRef<Faction> faction;

		
		[NoTranslate]
		public SlateRef<string> storeProcessedTagAs;
	}
}
