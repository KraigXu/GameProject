using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008DE RID: 2270
	public class MainButtonDef : Def
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x00126C45 File Offset: 0x00124E45
		public MainButtonWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (MainButtonWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003663 RID: 13923 RVA: 0x00126C77 File Offset: 0x00124E77
		public MainTabWindow TabWindow
		{
			get
			{
				if (this.tabWindowInt == null && this.tabWindowClass != null)
				{
					this.tabWindowInt = (MainTabWindow)Activator.CreateInstance(this.tabWindowClass);
					this.tabWindowInt.def = this;
				}
				return this.tabWindowInt;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x00126CB8 File Offset: 0x00124EB8
		public string ShortenedLabelCap
		{
			get
			{
				if (this.cachedShortenedLabelCap == null)
				{
					this.cachedShortenedLabelCap = base.LabelCap.Shorten();
				}
				return this.cachedShortenedLabelCap;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003665 RID: 13925 RVA: 0x00126CEC File Offset: 0x00124EEC
		public float LabelCapWidth
		{
			get
			{
				if (this.cachedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedLabelCapWidth = Text.CalcSize(base.LabelCap).x;
					Text.Font = font;
				}
				return this.cachedLabelCapWidth;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x00126D2C File Offset: 0x00124F2C
		public float ShortenedLabelCapWidth
		{
			get
			{
				if (this.cachedShortenedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedShortenedLabelCapWidth = Text.CalcSize(this.ShortenedLabelCap).x;
					Text.Font = font;
				}
				return this.cachedShortenedLabelCapWidth;
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003667 RID: 13927 RVA: 0x00126D67 File Offset: 0x00124F67
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null && this.iconPath != null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00126D97 File Offset: 0x00124F97
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00126DBA File Offset: 0x00124FBA
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00126DDD File Offset: 0x00124FDD
		public void Notify_ClearingAllMapsMemory()
		{
			this.tabWindowInt = null;
		}

		// Token: 0x04001EDA RID: 7898
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		// Token: 0x04001EDB RID: 7899
		public Type tabWindowClass;

		// Token: 0x04001EDC RID: 7900
		public bool buttonVisible = true;

		// Token: 0x04001EDD RID: 7901
		public int order;

		// Token: 0x04001EDE RID: 7902
		public KeyCode defaultHotKey;

		// Token: 0x04001EDF RID: 7903
		public bool canBeTutorDenied = true;

		// Token: 0x04001EE0 RID: 7904
		public bool validWithoutMap;

		// Token: 0x04001EE1 RID: 7905
		public bool minimized;

		// Token: 0x04001EE2 RID: 7906
		public string iconPath;

		// Token: 0x04001EE3 RID: 7907
		[Unsaved(false)]
		public KeyBindingDef hotKey;

		// Token: 0x04001EE4 RID: 7908
		[Unsaved(false)]
		public string cachedTutorTag;

		// Token: 0x04001EE5 RID: 7909
		[Unsaved(false)]
		public string cachedHighlightTagClosed;

		// Token: 0x04001EE6 RID: 7910
		[Unsaved(false)]
		private MainButtonWorker workerInt;

		// Token: 0x04001EE7 RID: 7911
		[Unsaved(false)]
		private MainTabWindow tabWindowInt;

		// Token: 0x04001EE8 RID: 7912
		[Unsaved(false)]
		private string cachedShortenedLabelCap;

		// Token: 0x04001EE9 RID: 7913
		[Unsaved(false)]
		private float cachedLabelCapWidth = -1f;

		// Token: 0x04001EEA RID: 7914
		[Unsaved(false)]
		private float cachedShortenedLabelCapWidth = -1f;

		// Token: 0x04001EEB RID: 7915
		[Unsaved(false)]
		private Texture2D icon;

		// Token: 0x04001EEC RID: 7916
		public const int ButtonHeight = 35;
	}
}
