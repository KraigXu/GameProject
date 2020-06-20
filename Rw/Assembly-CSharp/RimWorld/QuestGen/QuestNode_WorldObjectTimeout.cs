using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x02001198 RID: 4504
	public class QuestNode_WorldObjectTimeout : QuestNode_Delay
	{
		// Token: 0x0600684E RID: 26702 RVA: 0x0024701C File Offset: 0x0024521C
		protected override QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_WorldObjectTimeout
			{
				worldObject = this.worldObject.GetValue(QuestGen.slate)
			};
		}

		// Token: 0x040040A4 RID: 16548
		public SlateRef<WorldObject> worldObject;
	}
}
