using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003D4 RID: 980
	public class WidgetRow
	{
		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x000AF43D File Offset: 0x000AD63D
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x000AF445 File Offset: 0x000AD645
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000AF44D File Offset: 0x000AD64D
		public WidgetRow()
		{
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000AF467 File Offset: 0x000AD667
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000AF48E File Offset: 0x000AD68E
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000AF4BC File Offset: 0x000AD6BC
		private float LeftX(float elementWidth)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				return this.curX;
			}
			return this.curX - elementWidth;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000AF4E0 File Offset: 0x000AD6E0
		private void IncrementPosition(float amount)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				this.curX += amount;
			}
			else
			{
				this.curX -= amount;
			}
			if (Mathf.Abs(this.curX - this.startX) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000AF540 File Offset: 0x000AD740
		private void IncrementY()
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.LeftThenUp)
			{
				this.curY -= 24f + this.gap;
			}
			else
			{
				this.curY += 24f + this.gap;
			}
			this.curX = this.startX;
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000AF59E File Offset: 0x000AD79E
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000AF5C7 File Offset: 0x000AD7C7
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000AF5E0 File Offset: 0x000AD7E0
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null, bool doMouseoverSound = true)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			bool result = Widgets.ButtonImage(rect, tex, Color.white, mouseoverColor ?? GenUI.MouseoverColor, true);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000AF670 File Offset: 0x000AD870
		public void GapButtonIcon()
		{
			if (this.curY != this.startX)
			{
				this.IncrementPosition(24f + this.gap);
			}
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000AF694 File Offset: 0x000AD894
		public void ToggleableIcon(ref bool toggleable, Texture2D tex, string tooltip, SoundDef mouseoverSound = null, string tutorTag = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool flag = Widgets.ButtonImage(rect, tex, true);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Rect position = new Rect(rect.x + rect.width / 2f, rect.y, rect.height / 2f, rect.height / 2f);
			Texture2D image = toggleable ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect, mouseoverSound);
			}
			if (flag)
			{
				toggleable = !toggleable;
				if (toggleable)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, tutorTag);
			}
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000AF78C File Offset: 0x000AD98C
		public Rect Icon(Texture2D tex, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			GUI.DrawTexture(rect, tex);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000AF7F4 File Offset: 0x000AD9F4
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = true)
		{
			Vector2 vector = Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect rect = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, true);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(rect.width + this.gap);
			return result;
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000AF88C File Offset: 0x000ADA8C
		public Rect Label(string text, float width = -1f)
		{
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			this.IncrementPosition(2f);
			Widgets.Label(rect, text);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000AF8FC File Offset: 0x000ADAFC
		public Rect FillableBar(float width, float height, float fillPct, string label, Texture2D fillTex, Texture2D bgTex = null)
		{
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.FillableBar(rect, fillPct, fillTex, bgTex, false);
			if (!label.NullOrEmpty())
			{
				Rect rect2 = rect;
				rect2.xMin += 2f;
				rect2.xMax -= 2f;
				if (Text.Anchor >= TextAnchor.UpperLeft)
				{
					rect2.height += 14f;
				}
				Text.Font = GameFont.Tiny;
				Text.WordWrap = false;
				Widgets.Label(rect2, label);
				Text.WordWrap = true;
			}
			this.IncrementPosition(width);
			return rect;
		}

		// Token: 0x04001163 RID: 4451
		private float startX;

		// Token: 0x04001164 RID: 4452
		private float curX;

		// Token: 0x04001165 RID: 4453
		private float curY;

		// Token: 0x04001166 RID: 4454
		private float maxWidth = 99999f;

		// Token: 0x04001167 RID: 4455
		private float gap;

		// Token: 0x04001168 RID: 4456
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x04001169 RID: 4457
		public const float IconSize = 24f;

		// Token: 0x0400116A RID: 4458
		public const float DefaultGap = 4f;

		// Token: 0x0400116B RID: 4459
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x0400116C RID: 4460
		public const float LabelGap = 2f;

		// Token: 0x0400116D RID: 4461
		public const float ButtonExtraSpace = 16f;
	}
}
