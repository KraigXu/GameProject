using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EB RID: 1003
	public abstract class EditWindow : Window
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x000B6823 File Offset: 0x000B4A23
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x000B6834 File Offset: 0x000B4A34
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000B683C File Offset: 0x000B4A3C
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000B688C File Offset: 0x000B4A8C
		public override void PostOpen()
		{
			while (this.windowRect.x <= (float)UI.screenWidth - 200f && this.windowRect.y <= (float)UI.screenHeight - 200f)
			{
				bool flag = false;
				foreach (EditWindow editWindow in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (editWindow != this && Mathf.Abs(editWindow.windowRect.x - this.windowRect.x) < 8f && Mathf.Abs(editWindow.windowRect.y - this.windowRect.y) < 8f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				this.windowRect.x = this.windowRect.x + 16f;
				this.windowRect.y = this.windowRect.y + 16f;
			}
		}

		// Token: 0x04001228 RID: 4648
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04001229 RID: 4649
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x0400122A RID: 4650
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
