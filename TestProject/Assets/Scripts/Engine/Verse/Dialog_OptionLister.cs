using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class Dialog_OptionLister : Window
	{
		
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x000A0AE8 File Offset: 0x0009ECE8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			this.filter = Widgets.TextField(new Rect(0f, 0f, 200f, 30f), this.filter);
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 35f;
			int num = (int)(this.InitialSize.x / 200f);
			float num2 = (this.totalOptionsHeight + 24f * (float)(num - 1)) / (float)num;
			if (num2 < outRect.height)
			{
				num2 = outRect.height;
			}
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, num2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			this.listing = new Listing_Standard();
			this.listing.ColumnWidth = (rect.width - 17f * (float)(num - 1)) / (float)num;
			this.listing.Begin(rect);
			this.DoListingItems();
			this.listing.End();
			Widgets.EndScrollView();
		}

		
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		
		protected abstract void DoListingItems();

		
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		
		protected Vector2 scrollPosition;

		
		protected string filter = "";

		
		protected float totalOptionsHeight;

		
		protected Listing_Standard listing;
	}
}
