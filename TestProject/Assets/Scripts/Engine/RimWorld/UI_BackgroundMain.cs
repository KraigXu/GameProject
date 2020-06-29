using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		
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

		
		public void FadeOut()
		{
			this.fadeIn = false;
		}

		
		public void SetOverlayImage(Texture2D texture)
		{
			if (texture != null)
			{
				this.overlayImage = texture;
				this.fadeIn = true;
			}
		}

		
		private Color curColor = new Color(1f, 1f, 1f, 0f);

		
		private Texture2D overlayImage;

		
		public Texture2D overrideBGImage;

		
		private bool fadeIn;

		
		private const float DeltaAlpha = 0.04f;

		
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);
	}
}
