using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005AB RID: 1451
	public abstract class ThinkNode
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000EF80C File Offset: 0x000EDA0C
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000EF814 File Offset: 0x000EDA14
		public IEnumerable<ThinkNode> ThisAndChildrenRecursive
		{
			get
			{
				yield return this;
				foreach (ThinkNode thinkNode in this.ChildrenRecursive)
				{
					yield return thinkNode;
				}
				IEnumerator<ThinkNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000EF824 File Offset: 0x000EDA24
		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				int num;
				for (int i = 0; i < this.subNodes.Count; i = num + 1)
				{
					foreach (ThinkNode thinkNode in this.subNodes[i].ThisAndChildrenRecursive)
					{
						yield return thinkNode;
					}
					IEnumerator<ThinkNode> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000EF834 File Offset: 0x000EDA34
		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
				return 0f;
			}
			return this.priority;
		}

		// Token: 0x060028B9 RID: 10425
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		// Token: 0x060028BA RID: 10426 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ResolveSubnodes()
		{
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000EF868 File Offset: 0x000EDA68
		public void ResolveSubnodesAndRecur()
		{
			if (this.uniqueSaveKeyInt != -2)
			{
				return;
			}
			this.ResolveSubnodes();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				this.subNodes[i].ResolveSubnodesAndRecur();
			}
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000EF8B0 File Offset: 0x000EDAB0
		public virtual ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode thinkNode = (ThinkNode)Activator.CreateInstance(base.GetType());
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				thinkNode.subNodes.Add(this.subNodes[i].DeepCopy(resolve));
			}
			thinkNode.priority = this.priority;
			thinkNode.leaveJoinableLordIfIssuesJob = this.leaveJoinableLordIfIssuesJob;
			thinkNode.uniqueSaveKeyInt = this.uniqueSaveKeyInt;
			if (resolve)
			{
				thinkNode.ResolveSubnodesAndRecur();
			}
			ThinkTreeKeyAssigner.AssignSingleKey(thinkNode, 0);
			return thinkNode;
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000EF936 File Offset: 0x000EDB36
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000EF93F File Offset: 0x000EDB3F
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		// Token: 0x04001868 RID: 6248
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		// Token: 0x04001869 RID: 6249
		public bool leaveJoinableLordIfIssuesJob;

		// Token: 0x0400186A RID: 6250
		protected float priority = -1f;

		// Token: 0x0400186B RID: 6251
		[Unsaved(false)]
		private int uniqueSaveKeyInt = -2;

		// Token: 0x0400186C RID: 6252
		[Unsaved(false)]
		public ThinkNode parent;

		// Token: 0x0400186D RID: 6253
		public const int InvalidSaveKey = -1;

		// Token: 0x0400186E RID: 6254
		protected const int UnresolvedSaveKey = -2;
	}
}
