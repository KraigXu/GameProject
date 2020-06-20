using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000F7 RID: 247
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x060006D9 RID: 1753 RVA: 0x0001F9FB File Offset: 0x0001DBFB
		public static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001FA08 File Offset: 0x0001DC08
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001FA64 File Offset: 0x0001DC64
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001FA80 File Offset: 0x0001DC80
		private static int NextUnusedKeyFor(ThinkNode node)
		{
			int num = 0;
			while (node != null)
			{
				num = Gen.HashCombineInt(num, GenText.StableStringHash(node.GetType().Name));
				node = node.parent;
			}
			while (ThinkTreeKeyAssigner.assignedKeys.Contains(num))
			{
				num ^= Rand.Int;
			}
			ThinkTreeKeyAssigner.assignedKeys.Add(num);
			return num;
		}

		// Token: 0x04000648 RID: 1608
		private static HashSet<int> assignedKeys = new HashSet<int>();
	}
}
