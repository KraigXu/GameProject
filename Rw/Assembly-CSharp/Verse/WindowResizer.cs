using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EF RID: 1007
	public class WindowResizer
	{
		// Token: 0x06001DCF RID: 7631 RVA: 0x000B6FC0 File Offset: 0x000B51C0
		public Rect DoResizeControl(Rect winRect)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(winRect.width - 24f, winRect.height - 24f, 24f, 24f);
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.isResizing = true;
				this.resizeStart = new Rect(mousePosition.x, mousePosition.y, winRect.width, winRect.height);
			}
			if (this.isResizing)
			{
				winRect.width = this.resizeStart.width + (mousePosition.x - this.resizeStart.x);
				winRect.height = this.resizeStart.height + (mousePosition.y - this.resizeStart.y);
				if (winRect.width < this.minWindowSize.x)
				{
					winRect.width = this.minWindowSize.x;
				}
				if (winRect.height < this.minWindowSize.y)
				{
					winRect.height = this.minWindowSize.y;
				}
				winRect.xMax = Mathf.Min((float)UI.screenWidth, winRect.xMax);
				winRect.yMax = Mathf.Min((float)UI.screenHeight, winRect.yMax);
				if (Event.current.type == EventType.MouseUp)
				{
					this.isResizing = false;
				}
			}
			Widgets.ButtonImage(rect, TexUI.WinExpandWidget, true);
			return new Rect(winRect.x, winRect.y, (float)((int)winRect.width), (float)((int)winRect.height));
		}

		// Token: 0x04001253 RID: 4691
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x04001254 RID: 4692
		private bool isResizing;

		// Token: 0x04001255 RID: 4693
		private Rect resizeStart;

		// Token: 0x04001256 RID: 4694
		private const float ResizeButtonSize = 24f;
	}
}
