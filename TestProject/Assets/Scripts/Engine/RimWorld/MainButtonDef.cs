using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class MainButtonDef : Def
	{
		
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

		
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}

		
		public void Notify_ClearingAllMapsMemory()
		{
			this.tabWindowInt = null;
		}

		
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		
		public Type tabWindowClass;

		
		public bool buttonVisible = true;

		
		public int order;

		
		public KeyCode defaultHotKey;

		
		public bool canBeTutorDenied = true;

		
		public bool validWithoutMap;

		
		public bool minimized;

		
		public string iconPath;

		
		[Unsaved(false)]
		public KeyBindingDef hotKey;

		
		[Unsaved(false)]
		public string cachedTutorTag;

		
		[Unsaved(false)]
		public string cachedHighlightTagClosed;

		
		[Unsaved(false)]
		private MainButtonWorker workerInt;

		
		[Unsaved(false)]
		private MainTabWindow tabWindowInt;

		
		[Unsaved(false)]
		private string cachedShortenedLabelCap;

		
		[Unsaved(false)]
		private float cachedLabelCapWidth = -1f;

		
		[Unsaved(false)]
		private float cachedShortenedLabelCapWidth = -1f;

		
		[Unsaved(false)]
		private Texture2D icon;

		
		public const int ButtonHeight = 35;
	}
}
