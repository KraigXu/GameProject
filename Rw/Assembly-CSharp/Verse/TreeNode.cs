using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003AF RID: 943
	public class TreeNode
	{
		// Token: 0x06001BC4 RID: 7108 RVA: 0x000A9D4A File Offset: 0x000A7F4A
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000A9D57 File Offset: 0x000A7F57
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
				return;
			}
			this.openBits &= ~mask;
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001062 RID: 4194
		public TreeNode parentNode;

		// Token: 0x04001063 RID: 4195
		public List<TreeNode> children;

		// Token: 0x04001064 RID: 4196
		public int nestDepth;

		// Token: 0x04001065 RID: 4197
		private int openBits;
	}
}
