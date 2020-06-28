using System;
using System.Collections.Generic;

namespace RimWorld.QuestGen
{
	// Token: 0x0200110B RID: 4363
	public class QuestNode_Sequence : QuestNode
	{
		// Token: 0x06006651 RID: 26193 RVA: 0x0023D728 File Offset: 0x0023B928
		protected override void RunInt()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].Run();
			}
		}

		// Token: 0x06006652 RID: 26194 RVA: 0x0023D75C File Offset: 0x0023B95C
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (!this.nodes[i].TestRun(slate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003E69 RID: 15977
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
