using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class StandardLetter : ChoiceLetter
	{
		
		
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				if (this.quest != null)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", false);
				}
				if (!this.hyperlinkThingDefs.NullOrEmpty<ThingDef>())
				{
					int num;
					for (int i = 0; i < this.hyperlinkThingDefs.Count; i = num + 1)
					{
						yield return base.Option_ViewInfoCard(i);
						num = i;
					}
				}
				if (!this.hyperlinkHediffDefs.NullOrEmpty<HediffDef>())
				{
					int i = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
					int num;
					for (int j = 0; j < this.hyperlinkHediffDefs.Count; j = num + 1)
					{
						yield return base.Option_ViewInfoCard(i + j);
						num = j;
					}
				}
				yield break;
			}
		}
	}
}
