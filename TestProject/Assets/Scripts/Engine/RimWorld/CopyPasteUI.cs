using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E14 RID: 3604
	public static class CopyPasteUI
	{
		// Token: 0x06005729 RID: 22313 RVA: 0x001CFDE0 File Offset: 0x001CDFE0
		public static void DoCopyPasteButtons(Rect rect, Action copyAction, Action pasteAction)
		{
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height / 2f - 12f), 18f, 24f);
			if (Widgets.ButtonImage(rect2, TexButton.Copy, true))
			{
				copyAction();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect2, "Copy");
			if (pasteAction != null)
			{
				Rect rect3 = rect2;
				rect3.x = rect2.xMax;
				if (Widgets.ButtonImage(rect3, TexButton.Paste, true))
				{
					pasteAction();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "Paste");
			}
		}

		// Token: 0x04002F91 RID: 12177
		public const float CopyPasteIconHeight = 24f;

		// Token: 0x04002F92 RID: 12178
		public const float CopyPasteIconWidth = 18f;

		// Token: 0x04002F93 RID: 12179
		public const float CopyPasteColumnWidth = 36f;
	}
}
