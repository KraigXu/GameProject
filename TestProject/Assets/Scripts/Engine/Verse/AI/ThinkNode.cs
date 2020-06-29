﻿using System;
using System.Collections.Generic;

namespace Verse.AI
{
	
	public abstract class ThinkNode
	{
		
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000EF80C File Offset: 0x000EDA0C
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		
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

		
		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
				return 0f;
			}
			return this.priority;
		}

		
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		
		protected virtual void ResolveSubnodes()
		{
		}

		
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

		
		public virtual void ResolveReferences()
		{
		}

		
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

		
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		
		public bool leaveJoinableLordIfIssuesJob;

		
		protected float priority = -1f;

		
		[Unsaved(false)]
		private int uniqueSaveKeyInt = -2;

		
		[Unsaved(false)]
		public ThinkNode parent;

		
		public const int InvalidSaveKey = -1;

		
		protected const int UnresolvedSaveKey = -2;
	}
}
