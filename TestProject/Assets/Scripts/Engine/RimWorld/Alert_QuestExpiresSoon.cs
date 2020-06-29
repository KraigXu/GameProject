using System;
using Verse;

namespace RimWorld
{
	
	public class Alert_QuestExpiresSoon : Alert
	{
		
		// (get) Token: 0x06005630 RID: 22064 RVA: 0x001C8FC4 File Offset: 0x001C71C4
		private Quest QuestExpiring
		{
			get
			{
				foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
				{
					if (!quest.dismissed && !quest.Historical && !quest.initiallyAccepted && quest.State == QuestState.NotYetAccepted && quest.ticksUntilAcceptanceExpiry > 0 && quest.ticksUntilAcceptanceExpiry < 60000)
					{
						return quest;
					}
				}
				return null;
			}
		}

		
		public Alert_QuestExpiresSoon()
		{
			this.defaultPriority = AlertPriority.High;
		}

		
		protected override void OnClick()
		{
			if (this.QuestExpiring != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.QuestExpiring);
			}
		}

		
		public override string GetLabel()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoon".Translate(questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
		}

		
		public override TaggedString GetExplanation()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoonDesc".Translate(questExpiring.name, questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
		}

		
		public override AlertReport GetReport()
		{
			return this.QuestExpiring != null;
		}

		
		private const int TicksToAlert = 60000;
	}
}
