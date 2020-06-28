using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C9 RID: 969
	public static class OptionListingUtility
	{
		// Token: 0x06001C94 RID: 7316 RVA: 0x000ADC4C File Offset: 0x000ABE4C
		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption listableOption in optList)
			{
				num += listableOption.DrawOption(new Vector2(0f, num), rect.width) + 7f;
			}
			GUI.EndGroup();
			return num;
		}

		// Token: 0x040010DA RID: 4314
		private const float OptionSpacing = 7f;
	}
}
