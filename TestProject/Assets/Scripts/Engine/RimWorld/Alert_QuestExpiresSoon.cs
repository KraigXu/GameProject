using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE1 RID: 3553
	public class Alert_QuestExpiresSoon : Alert
	{
		// Token: 0x17000F61 RID: 3937
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

		// Token: 0x06005631 RID: 22065 RVA: 0x001C8E4B File Offset: 0x001C704B
		public Alert_QuestExpiresSoon()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x001C9054 File Offset: 0x001C7254
		protected override void OnClick()
		{
			if (this.QuestExpiring != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.QuestExpiring);
			}
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x001C9088 File Offset: 0x001C7288
		public override string GetLabel()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoon".Translate(questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x001C90C8 File Offset: 0x001C72C8
		public override TaggedString GetExplanation()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoonDesc".Translate(questExpiring.name, questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x001C911D File Offset: 0x001C731D
		public override AlertReport GetReport()
		{
			return this.QuestExpiring != null;
		}

		// Token: 0x04002F1C RID: 12060
		private const int TicksToAlert = 60000;
	}
}
