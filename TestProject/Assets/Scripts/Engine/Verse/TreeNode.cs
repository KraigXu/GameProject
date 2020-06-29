using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class TreeNode
	{
		
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
				return;
			}
			this.openBits &= ~mask;
		}

		
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		
		public TreeNode parentNode;

		
		public List<TreeNode> children;

		
		public int nestDepth;

		
		private int openBits;
	}
}
