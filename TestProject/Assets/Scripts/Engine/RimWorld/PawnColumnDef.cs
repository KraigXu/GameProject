using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class PawnColumnDef : Def
	{
		
		// (get) Token: 0x06003681 RID: 13953 RVA: 0x001275EF File Offset: 0x001257EF
		public PawnColumnWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnColumnWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		// (get) Token: 0x06003682 RID: 13954 RVA: 0x00127621 File Offset: 0x00125821
		public Texture2D HeaderIcon
		{
			get
			{
				if (this.headerIconTex == null && !this.headerIcon.NullOrEmpty())
				{
					this.headerIconTex = ContentFinder<Texture2D>.Get(this.headerIcon, true);
				}
				return this.headerIconTex;
			}
		}

		
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x00127658 File Offset: 0x00125858
		public Vector2 HeaderIconSize
		{
			get
			{
				if (this.headerIconSize != default(Vector2))
				{
					return this.headerIconSize;
				}
				if (this.HeaderIcon != null)
				{
					return PawnColumnDef.IconSize;
				}
				return Vector2.zero;
			}
		}

		
		// (get) Token: 0x06003684 RID: 13956 RVA: 0x0012769B File Offset: 0x0012589B
		public bool HeaderInteractable
		{
			get
			{
				return this.sortable || !this.headerTip.NullOrEmpty() || this.headerAlwaysInteractable;
			}
		}

		
		public Type workerClass = typeof(PawnColumnWorker);

		
		public bool sortable;

		
		public bool ignoreWhenCalculatingOptimalTableSize;

		
		[NoTranslate]
		public string headerIcon;

		
		public Vector2 headerIconSize;

		
		[MustTranslate]
		public string headerTip;

		
		public bool headerAlwaysInteractable;

		
		public bool paintable;

		
		public TrainableDef trainable;

		
		public int gap;

		
		public WorkTypeDef workType;

		
		public bool moveWorkTypeLabelDown;

		
		public int widthPriority;

		
		public int width = -1;

		
		[Unsaved(false)]
		private PawnColumnWorker workerInt;

		
		[Unsaved(false)]
		private Texture2D headerIconTex;

		
		private const int IconWidth = 26;

		
		private static readonly Vector2 IconSize = new Vector2(26f, 26f);
	}
}
