using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public static class CopyPasteUI
	{
		
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

		
		public const float CopyPasteIconHeight = 24f;

		
		public const float CopyPasteIconWidth = 18f;

		
		public const float CopyPasteColumnWidth = 36f;
	}
}
