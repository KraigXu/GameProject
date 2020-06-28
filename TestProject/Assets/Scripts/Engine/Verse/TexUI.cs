using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D1 RID: 977
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x0400113E RID: 4414
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x0400113F RID: 4415
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04001140 RID: 4416
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x04001141 RID: 4417
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x04001142 RID: 4418
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x04001143 RID: 4419
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x04001144 RID: 4420
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x04001145 RID: 4421
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x04001146 RID: 4422
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x04001147 RID: 4423
		public static readonly Texture2D GrayBg = SolidColorMaterials.NewSolidColorTexture(new ColorInt(51, 63, 51, 200).ToColor);

		// Token: 0x04001148 RID: 4424
		public static readonly Color AvailResearchColor = new ColorInt(32, 32, 32, 255).ToColor;

		// Token: 0x04001149 RID: 4425
		public static readonly Color ActiveResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x0400114A RID: 4426
		public static readonly Color FinishedResearchColor = new ColorInt(0, 64, 16, 255).ToColor;

		// Token: 0x0400114B RID: 4427
		public static readonly Color LockedResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x0400114C RID: 4428
		public static readonly Color RelatedResearchColor = new ColorInt(0, 0, 0, 255).ToColor;

		// Token: 0x0400114D RID: 4429
		public static readonly Color HighlightBgResearchColor = new ColorInt(30, 30, 30, 255).ToColor;

		// Token: 0x0400114E RID: 4430
		public static readonly Color HighlightBorderResearchColor = new ColorInt(160, 160, 160, 255).ToColor;

		// Token: 0x0400114F RID: 4431
		public static readonly Color DefaultBorderResearchColor = new ColorInt(80, 80, 80, 255).ToColor;

		// Token: 0x04001150 RID: 4432
		public static readonly Color DefaultLineResearchColor = new ColorInt(60, 60, 60, 255).ToColor;

		// Token: 0x04001151 RID: 4433
		public static readonly Color HighlightLineResearchColor = new ColorInt(51, 205, 217, 255).ToColor;

		// Token: 0x04001152 RID: 4434
		public static readonly Color DependencyOutlineResearchColor = new ColorInt(217, 20, 51, 255).ToColor;

		// Token: 0x04001153 RID: 4435
		public static readonly Texture2D FastFillTex = Texture2D.whiteTexture;

		// Token: 0x04001154 RID: 4436
		public static readonly GUIStyle FastFillStyle = new GUIStyle
		{
			normal = new GUIStyleState
			{
				background = TexUI.FastFillTex
			}
		};

		// Token: 0x04001155 RID: 4437
		public static readonly Texture2D TextBGBlack = ContentFinder<Texture2D>.Get("UI/Widgets/TextBGBlack", true);

		// Token: 0x04001156 RID: 4438
		public static readonly Texture2D GrayTextBG = ContentFinder<Texture2D>.Get("UI/Overlays/GrayTextBG", true);

		// Token: 0x04001157 RID: 4439
		public static readonly Texture2D FloatMenuOptionBG = ContentFinder<Texture2D>.Get("UI/Widgets/FloatMenuOptionBG", true);

		// Token: 0x04001158 RID: 4440
		public static readonly Material GrayscaleGUI = MatLoader.LoadMat("Misc/GrayscaleGUI", -1);
	}
}
