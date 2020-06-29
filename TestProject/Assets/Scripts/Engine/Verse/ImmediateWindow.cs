using System;
using UnityEngine;

namespace Verse
{
	
	public class ImmediateWindow : Window
	{
		
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x000B69C0 File Offset: 0x000B4BC0
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		
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

		
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		
		public Action doWindowFunc;
	}
}
