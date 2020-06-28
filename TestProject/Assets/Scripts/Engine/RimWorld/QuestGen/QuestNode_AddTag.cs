using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020010FF RID: 4351
	public class QuestNode_AddTag : QuestNode
	{
		// Token: 0x0600662B RID: 26155 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x0023C8F0 File Offset: 0x0023AAF0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate) == null)
			{
				return;
			}
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			foreach (object obj in this.targets.GetValue(slate))
			{
				Thing thing = obj as Thing;
				if (thing != null)
				{
					QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				}
				else
				{
					WorldObject worldObject = obj as WorldObject;
					if (worldObject != null)
					{
						QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
					}
				}
			}
		}

		// Token: 0x04003E37 RID: 15927
		[NoTranslate]
		public SlateRef<IEnumerable<object>> targets;

		// Token: 0x04003E38 RID: 15928
		[NoTranslate]
		public SlateRef<string> tag;
	}
}
