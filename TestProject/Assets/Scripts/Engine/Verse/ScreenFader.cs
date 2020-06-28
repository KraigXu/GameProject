using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000473 RID: 1139
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x000CD05C File Offset: 0x000CB25C
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000CD064 File Offset: 0x000CB264
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000CD108 File Offset: 0x000CB308
		public static void OverlayOnGUI(Vector2 windowSize)
		{
			Color color = ScreenFader.CurrentInstantColor();
			if (color.a > 0f)
			{
				if (ScreenFader.fadeTextureDirty)
				{
					ScreenFader.fadeTexture.SetPixel(0, 0, color);
					ScreenFader.fadeTexture.Apply();
				}
				GUI.Label(new Rect(-10f, -10f, windowSize.x + 10f, windowSize.y + 10f), ScreenFader.fadeTexture, ScreenFader.backgroundStyle);
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000CD17C File Offset: 0x000CB37C
		private static Color CurrentInstantColor()
		{
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				return ScreenFader.targetColor;
			}
			return Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x000CD1CD File Offset: 0x000CB3CD
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000CD1F5 File Offset: 0x000CB3F5
		public static void StartFade(Color finalColor, float duration)
		{
			if (duration <= 0f)
			{
				ScreenFader.SetColor(finalColor);
				return;
			}
			ScreenFader.sourceColor = ScreenFader.CurrentInstantColor();
			ScreenFader.targetColor = finalColor;
			ScreenFader.sourceTime = ScreenFader.CurTime;
			ScreenFader.targetTime = ScreenFader.CurTime + duration;
		}

		// Token: 0x04001498 RID: 5272
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04001499 RID: 5273
		private static Texture2D fadeTexture;

		// Token: 0x0400149A RID: 5274
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x0400149B RID: 5275
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x0400149C RID: 5276
		private static float sourceTime = 0f;

		// Token: 0x0400149D RID: 5277
		private static float targetTime = 0f;

		// Token: 0x0400149E RID: 5278
		private static bool fadeTextureDirty = true;
	}
}
