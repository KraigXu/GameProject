using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E1 RID: 993
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x000A0AE8 File Offset: 0x0009ECE8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000B55AC File Offset: 0x000B37AC
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000B55D4 File Offset: 0x000B37D4
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

		// Token: 0x06001D8C RID: 7564 RVA: 0x000B56F2 File Offset: 0x000B38F2
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x06001D8D RID: 7565
		protected abstract void DoListingItems();

		// Token: 0x06001D8E RID: 7566 RVA: 0x000B56FF File Offset: 0x000B38FF
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040011F3 RID: 4595
		protected Vector2 scrollPosition;

		// Token: 0x040011F4 RID: 4596
		protected string filter = "";

		// Token: 0x040011F5 RID: 4597
		protected float totalOptionsHeight;

		// Token: 0x040011F6 RID: 4598
		protected Listing_Standard listing;
	}
}
