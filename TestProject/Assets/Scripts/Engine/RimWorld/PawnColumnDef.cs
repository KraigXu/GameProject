using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008E6 RID: 2278
	public class PawnColumnDef : Def
	{
		// Token: 0x170009C0 RID: 2496
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

		// Token: 0x170009C1 RID: 2497
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

		// Token: 0x170009C2 RID: 2498
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

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003684 RID: 13956 RVA: 0x0012769B File Offset: 0x0012589B
		public bool HeaderInteractable
		{
			get
			{
				return this.sortable || !this.headerTip.NullOrEmpty() || this.headerAlwaysInteractable;
			}
		}

		// Token: 0x04001F0D RID: 7949
		public Type workerClass = typeof(PawnColumnWorker);

		// Token: 0x04001F0E RID: 7950
		public bool sortable;

		// Token: 0x04001F0F RID: 7951
		public bool ignoreWhenCalculatingOptimalTableSize;

		// Token: 0x04001F10 RID: 7952
		[NoTranslate]
		public string headerIcon;

		// Token: 0x04001F11 RID: 7953
		public Vector2 headerIconSize;

		// Token: 0x04001F12 RID: 7954
		[MustTranslate]
		public string headerTip;

		// Token: 0x04001F13 RID: 7955
		public bool headerAlwaysInteractable;

		// Token: 0x04001F14 RID: 7956
		public bool paintable;

		// Token: 0x04001F15 RID: 7957
		public TrainableDef trainable;

		// Token: 0x04001F16 RID: 7958
		public int gap;

		// Token: 0x04001F17 RID: 7959
		public WorkTypeDef workType;

		// Token: 0x04001F18 RID: 7960
		public bool moveWorkTypeLabelDown;

		// Token: 0x04001F19 RID: 7961
		public int widthPriority;

		// Token: 0x04001F1A RID: 7962
		public int width = -1;

		// Token: 0x04001F1B RID: 7963
		[Unsaved(false)]
		private PawnColumnWorker workerInt;

		// Token: 0x04001F1C RID: 7964
		[Unsaved(false)]
		private Texture2D headerIconTex;

		// Token: 0x04001F1D RID: 7965
		private const int IconWidth = 26;

		// Token: 0x04001F1E RID: 7966
		private static readonly Vector2 IconSize = new Vector2(26f, 26f);
	}
}
