using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200128A RID: 4746
	[StaticConstructorOnStartup]
	public static class CompassWidget
	{
		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06006F8D RID: 28557 RVA: 0x0026D460 File Offset: 0x0026B660
		private static float Angle
		{
			get
			{
				Vector2 vector = GenWorldUI.WorldToUIPosition(Find.WorldGrid.NorthPolePos);
				Vector2 a = new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
				vector.y = (float)UI.screenHeight - vector.y;
				return a.AngleTo(vector);
			}
		}

		// Token: 0x06006F8E RID: 28558 RVA: 0x0026D4B4 File Offset: 0x0026B6B4
		public static void CompassOnGUI(ref float curBaseY)
		{
			CompassWidget.CompassOnGUI(new Vector2((float)UI.screenWidth - 10f - 32f, curBaseY - 10f - 32f));
			curBaseY -= 84f;
		}

		// Token: 0x06006F8F RID: 28559 RVA: 0x0026D4EC File Offset: 0x0026B6EC
		private static void CompassOnGUI(Vector2 center)
		{
			Widgets.DrawTextureRotated(center, CompassWidget.CompassTex, CompassWidget.Angle, 1f);
			Rect rect = new Rect(center.x - 32f, center.y - 32f, 64f, 64f);
			TooltipHandler.TipRegionByKey(rect, "CompassTip");
			if (Widgets.ButtonInvisible(rect, true))
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
		}

		// Token: 0x04004485 RID: 17541
		private const float Padding = 10f;

		// Token: 0x04004486 RID: 17542
		private const float Size = 64f;

		// Token: 0x04004487 RID: 17543
		private static readonly Texture2D CompassTex = ContentFinder<Texture2D>.Get("UI/Misc/Compass", true);
	}
}
