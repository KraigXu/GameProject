using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_AddTag : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<IEnumerable<object>> targets;

		
		[NoTranslate]
		public SlateRef<string> tag;
	}
}
