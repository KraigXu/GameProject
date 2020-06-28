using System;

namespace Verse.AI
{
	// Token: 0x02000580 RID: 1408
	public class Pawn_Thinker
	{
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000ECDDC File Offset: 0x000EAFDC
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x000ECDEE File Offset: 0x000EAFEE
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x000ECE05 File Offset: 0x000EB005
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x000ECE17 File Offset: 0x000EB017
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000ECE2E File Offset: 0x000EB02E
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000ECE40 File Offset: 0x000EB040
		public T TryGetMainTreeThinkNode<T>() where T : ThinkNode
		{
			foreach (ThinkNode thinkNode in this.MainThinkNodeRoot.ChildrenRecursive)
			{
				T t = thinkNode as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000ECEAC File Offset: 0x000EB0AC
		public T GetMainTreeThinkNode<T>() where T : ThinkNode
		{
			T t = this.TryGetMainTreeThinkNode<T>();
			if (t == null)
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" looked for ThinkNode of type ",
					typeof(T),
					" and didn't find it."
				}), false);
			}
			return t;
		}

		// Token: 0x04001826 RID: 6182
		public Pawn pawn;
	}
}
