using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EC RID: 1004
	public class ImmediateWindow : Window
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x000B69C0 File Offset: 0x000B4BC0
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000B69D0 File Offset: 0x000B4BD0
		public ImmediateWindow()
		{
			this.doCloseButton = false;
			this.doCloseX = false;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.focusWhenOpened = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000B6A22 File Offset: 0x000B4C22
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x0400122B RID: 4651
		public Action doWindowFunc;
	}
}
