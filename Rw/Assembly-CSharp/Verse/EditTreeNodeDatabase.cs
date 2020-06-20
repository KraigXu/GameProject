using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003A1 RID: 929
	public static class EditTreeNodeDatabase
	{
		// Token: 0x06001B50 RID: 6992 RVA: 0x000A7310 File Offset: 0x000A5510
		public static TreeNode_Editor RootOf(object obj)
		{
			for (int i = 0; i < EditTreeNodeDatabase.roots.Count; i++)
			{
				if (EditTreeNodeDatabase.roots[i].obj == obj)
				{
					return EditTreeNodeDatabase.roots[i];
				}
			}
			TreeNode_Editor treeNode_Editor = TreeNode_Editor.NewRootNode(obj);
			EditTreeNodeDatabase.roots.Add(treeNode_Editor);
			return treeNode_Editor;
		}

		// Token: 0x04001030 RID: 4144
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}
