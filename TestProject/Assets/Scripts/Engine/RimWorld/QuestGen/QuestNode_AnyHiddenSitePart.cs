using System;
using System.Collections.Generic;
using System.Linq;

namespace RimWorld.QuestGen
{
	// Token: 0x0200114B RID: 4427
	public class QuestNode_AnyHiddenSitePart : QuestNode
	{
		// Token: 0x0600674F RID: 26447 RVA: 0x00242CE3 File Offset: 0x00240EE3
		protected override bool TestRunInt(Slate slate)
		{
			if (this.AnyHiddenSitePart(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x00242D1B File Offset: 0x00240F1B
		protected override void RunInt()
		{
			if (this.AnyHiddenSitePart(QuestGen.slate))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x00242D54 File Offset: 0x00240F54
		private bool AnyHiddenSitePart(Slate slate)
		{
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				return value.Any((SitePartDef x) => x.defaultHidden);
			}
			return false;
		}

		// Token: 0x04003F81 RID: 16257
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04003F82 RID: 16258
		public QuestNode node;

		// Token: 0x04003F83 RID: 16259
		public QuestNode elseNode;
	}
}
