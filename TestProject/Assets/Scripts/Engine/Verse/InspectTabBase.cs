using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class InspectTabBase
	{
		
		// (get) Token: 0x06000393 RID: 915
		protected abstract float PaneTopY { get; }

		
		// (get) Token: 0x06000394 RID: 916
		protected abstract bool StillValid { get; }

		
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		
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

		
		protected abstract void CloseTab();

		
		protected abstract void FillTab();

		
		protected virtual void ExtraOnGUI()
		{
		}

		
		protected virtual void UpdateSize()
		{
		}

		
		public virtual void OnOpen()
		{
		}

		
		public virtual void TabTick()
		{
		}

		
		public virtual void TabUpdate()
		{
		}

		
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		
		public string labelKey;

		
		protected Vector2 size;

		
		public string tutorTag;

		
		private string cachedTutorHighlightTagClosed;
	}
}
