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
