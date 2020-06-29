using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public class NewQuestLetter : ChoiceLetter
	{
		
		// (get) Token: 0x06001B3F RID: 6975 RVA: 0x000A6F6B File Offset: 0x000A516B
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (this.quest != null)
				{
					yield return base.Option_ViewInQuestsTab("ViewQuest", false);
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				yield return base.Option_Close;
				yield break;
			}
		}

		
		public override void OpenLetter()
		{
			if (this.quest != null && !base.ArchivedOnly)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				Find.LetterStack.RemoveLetter(this);
				return;
			}
			base.OpenLetter();
		}
	}
}
