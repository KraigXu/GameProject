using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_WorldObjectTimeout : QuestNode_Delay
	{
		
		protected override QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_WorldObjectTimeout
			{
				worldObject = this.worldObject.GetValue(QuestGen.slate)
			};
		}

		
		public SlateRef<WorldObject> worldObject;
	}
}
