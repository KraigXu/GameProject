using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Dialog_FactionDuringLanding : Window
	{
		
		// (get) Token: 0x0600588A RID: 22666 RVA: 0x001270D1 File Offset: 0x001252D1
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		
		public Dialog_FactionDuringLanding()
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - this.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight);
		}

		
		private Vector2 scrollPosition = Vector2.zero;

		
		private float scrollViewHeight;
	}
}
