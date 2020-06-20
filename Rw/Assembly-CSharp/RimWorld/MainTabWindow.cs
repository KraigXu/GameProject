using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008E0 RID: 2272
	public abstract class MainTabWindow : Window
	{
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x001270D1 File Offset: 0x001252D1
		public virtual Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Left;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x001270E4 File Offset: 0x001252E4
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 requestedTabSize = this.RequestedTabSize;
				if (requestedTabSize.y > (float)(UI.screenHeight - 35))
				{
					requestedTabSize.y = (float)(UI.screenHeight - 35);
				}
				if (requestedTabSize.x > (float)UI.screenWidth)
				{
					requestedTabSize.x = (float)UI.screenWidth;
				}
				return requestedTabSize;
			}
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x00127135 File Offset: 0x00125335
		public MainTabWindow()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = SoundDefOf.TabClose;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000B6F99 File Offset: 0x000B5199
		public override void DoWindowContents(Rect inRect)
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x0012716C File Offset: 0x0012536C
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			if (this.Anchor == MainTabWindowAnchor.Left)
			{
				this.windowRect.x = 0f;
			}
			else
			{
				this.windowRect.x = (float)UI.screenWidth - this.windowRect.width;
			}
			this.windowRect.y = (float)(UI.screenHeight - 35) - this.windowRect.height;
		}

		// Token: 0x04001EF0 RID: 7920
		public MainButtonDef def;
	}
}
