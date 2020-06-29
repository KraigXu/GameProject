using System;

namespace Verse.AI
{
	
	public class Pawn_Thinker
	{
		
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000ECDDC File Offset: 0x000EAFDC
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x000ECDEE File Offset: 0x000EAFEE
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x000ECE05 File Offset: 0x000EB005
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x000ECE17 File Offset: 0x000EB017
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
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

		
		public Pawn pawn;
	}
}
