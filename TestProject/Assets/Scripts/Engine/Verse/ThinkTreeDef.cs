using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public class ThinkTreeDef : Def
	{

		public override void ResolveReferences()
		{
			this.thinkRoot.ResolveSubnodesAndRecur();
			foreach (ThinkNode thinkNode in this.thinkRoot.ThisAndChildrenRecursive)
			{
				thinkNode.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(this.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}


		public override IEnumerable<string> ConfigErrors()
		{
			IEnumerator<string> enumerator = null;
			HashSet<int> usedKeys = new HashSet<int>();
			HashSet<ThinkNode> instances = new HashSet<ThinkNode>();
			foreach (ThinkNode node in this.thinkRoot.ThisAndChildrenRecursive)
			{
				int key = node.UniqueSaveKey;
				if (key == -1)
				{
					yield return string.Concat(new object[]
					{
						"Thinknode ",
						node.GetType(),
						" has invalid save key ",
						key
					});
				}
				else if (instances.Contains(node))
				{
					yield return "There are two same ThinkNode instances in one think tree (their type is " + node.GetType() + ")";
				}
				else if (usedKeys.Contains(key))
				{
					yield return string.Concat(new object[]
					{
						"Two ThinkNodes have the same unique save key ",
						key,
						" (one of the nodes is ",
						node.GetType(),
						")"
					});
				}
				if (key != -1)
				{
					usedKeys.Add(key);
				}
				instances.Add(node);
				
			}
			yield break;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001FB70 File Offset: 0x0001DD70
		public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
		{
			outNode = null;
			if (key == -1)
			{
				return false;
			}
			if (key == this.thinkRoot.UniqueSaveKey)
			{
				outNode = this.thinkRoot;
				return true;
			}
			foreach (ThinkNode thinkNode in this.thinkRoot.ChildrenRecursive)
			{
				if (thinkNode.UniqueSaveKey == key)
				{
					outNode = thinkNode;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001FBF0 File Offset: 0x0001DDF0
		private void ResolveParentNodes(ThinkNode node)
		{
			for (int i = 0; i < node.subNodes.Count; i++)
			{
				if (node.subNodes[i].parent != null)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Think node ",
						node.subNodes[i],
						" from think tree ",
						this.defName,
						" already has a parent node (",
						node.subNodes[i].parent,
						"). This means that it's referenced by more than one think tree (should have been copied instead)."
					}), false);
				}
				else
				{
					node.subNodes[i].parent = node;
					this.ResolveParentNodes(node.subNodes[i]);
				}
			}
		}

		// Token: 0x04000649 RID: 1609
		public ThinkNode thinkRoot;

		// Token: 0x0400064A RID: 1610
		[NoTranslate]
		public string insertTag;

		// Token: 0x0400064B RID: 1611
		public float insertPriority;
	}
}
