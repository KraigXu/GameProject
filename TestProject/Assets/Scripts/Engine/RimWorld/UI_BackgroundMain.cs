using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E91 RID: 3729
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		// Token: 0x06005AEA RID: 23274 RVA: 0x001EFD98 File Offset: 0x001EDF98
		public override void BackgroundOnGUI()
		{
			Vector2 vector = (this.overrideBGImage != null) ? new Vector2((float)this.overrideBGImage.width, (float)this.overrideBGImage.height) : UI_BackgroundMain.BGPlanetSize;
			bool flag = true;
			if ((float)UI.screenWidth > (float)UI.screenHeight * (vector.x / vector.y))
			{
				flag = false;
			}
			Rect rect;
			if (flag)
			{
				float height = (float)UI.screenHeight;
				float num = (float)UI.screenHeight * (vector.x / vector.y);
				rect = new Rect((float)(UI.screenWidth / 2) - num / 2f, 0f, num, height);
			}
			else
			{
				float width = (float)UI.screenWidth;
				float num2 = (float)UI.screenWidth * (vector.y / vector.x);
				rect = new Rect(0f, (float)(UI.screenHeight / 2) - num2 / 2f, width, num2);
			}
			GUI.DrawTexture(rect, this.overrideBGImage ?? UI_BackgroundMain.BGPlanet, ScaleMode.ScaleToFit);
			this.DoOverlay(rect);
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x001EFE98 File Offset: 0x001EE098
		private void DoOverlay(Rect bgRect)
		{
			if (this.overlayImage != null)
			{
				if (this.fadeIn && this.curColor.a < 1f)
				{
					this.curColor.a = this.curColor.a + 0.04f;
				}
				else if (this.curColor.a > 0f)
				{
					this.curColor.a = this.curColor.a - 0.04f;
				}
				this.curColor.a = Mathf.Clamp01(this.curColor.a);
				GUI.color = this.curColor;
				GUI.DrawTexture(bgRect, this.overlayImage, ScaleMode.ScaleAndCrop);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x001EFF49 File Offset: 0x001EE149
		public void FadeOut()
		{
			this.fadeIn = false;
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x001EFF52 File Offset: 0x001EE152
		public void SetOverlayImage(Texture2D texture)
		{
			if (texture != null)
			{
				this.overlayImage = texture;
				this.fadeIn = true;
			}
		}

		// Token: 0x04003196 RID: 12694
		private Color curColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04003197 RID: 12695
		private Texture2D overlayImage;

		// Token: 0x04003198 RID: 12696
		public Texture2D overrideBGImage;

		// Token: 0x04003199 RID: 12697
		private bool fadeIn;

		// Token: 0x0400319A RID: 12698
		private const float DeltaAlpha = 0.04f;

		// Token: 0x0400319B RID: 12699
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		// Token: 0x0400319C RID: 12700
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);
	}
}
