using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000049 RID: 73
	public abstract class InspectTabBase
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000393 RID: 915
		protected abstract float PaneTopY { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000394 RID: 916
		protected abstract bool StillValid { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00012D75 File Offset: 0x00010F75
		public string TutorHighlightTagClosed
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorHighlightTagClosed == null)
				{
					this.cachedTutorHighlightTagClosed = "ITab-" + this.tutorTag + "-Closed";
				}
				return this.cachedTutorHighlightTagClosed;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00012DAC File Offset: 0x00010FAC
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00012DFC File Offset: 0x00010FFC
		public void DoTabGUI()
		{
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, delegate
			{
				if (!this.StillValid || !this.IsVisible)
				{
					return;
				}
				if (Widgets.CloseButtonFor(rect.AtZero()))
				{
					this.CloseTab();
				}
				try
				{
					this.FillTab();
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception filling tab ",
						this.GetType(),
						": ",
						ex
					}), 49827, false);
				}
			}, true, false, 1f);
			this.ExtraOnGUI();
		}

		// Token: 0x06000399 RID: 921
		protected abstract void CloseTab();

		// Token: 0x0600039A RID: 922
		protected abstract void FillTab();

		// Token: 0x0600039B RID: 923 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void OnOpen()
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void TabTick()
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void TabUpdate()
		{
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		// Token: 0x04000100 RID: 256
		public string labelKey;

		// Token: 0x04000101 RID: 257
		protected Vector2 size;

		// Token: 0x04000102 RID: 258
		public string tutorTag;

		// Token: 0x04000103 RID: 259
		private string cachedTutorHighlightTagClosed;
	}
}
